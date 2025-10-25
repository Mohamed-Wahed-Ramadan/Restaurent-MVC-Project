// تهيئة EmailJS
emailjs.init("5lHMXpcGuc52IQgst");

// متغير لحفظ رمز التحقق
let verificationCode = null;
let isEmailVerified = false;

// دالة لعرض الإشعارات
function showNotification(message, type = 'info') {
    const alertClass = type === 'success' ? 'alert-success' : type === 'error' ? 'alert-danger' : 'alert-info';
    const iconClass = type === 'success' ? 'fa-check-circle' : type === 'error' ? 'fa-exclamation-circle' : 'fa-info-circle';

    const alertHtml = `
        <div class="alert ${alertClass} alert-dismissible fade show" role="alert">
            <i class="fas ${iconClass} me-2"></i> ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        </div>
    `;

    $('.card-body').prepend(alertHtml);

    setTimeout(function () {
        $('.alert-dismissible').fadeOut(300, function () {
            $(this).remove();
        });
    }, 5000);
}

// التأكد من أن الصفحة محملة بالكامل
$(document).ready(function () {
    console.log("Page loaded successfully");

    // السماح بالحروف والمسافات فقط في الاسم الكامل (بدون أرقام)
    $('#FullName').on('keypress', function (e) {
        // السماح بالحروف العربية والإنجليزية والمسافات فقط
        const isLetter = /[a-zA-Z\u0600-\u06FF\s]/.test(e.key);
        if (!isLetter && e.key !== ' ') {
            e.preventDefault();
        }
    });

    // السماح بالأرقام فقط في رمز التحقق
    $('#verificationCode').on('input', function () {
        this.value = this.value.replace(/\D/g, '');
    });

    // زر التحقق من البريد الإلكتروني
    $('#verifyBtn').on('click', function () {
        const verifyBtn = $(this);
        const email = $('#Email').val().trim();
        const verifyStatus = $('#verifyStatus');

        console.log("Verify button clicked, email:", email);

        // التحقق من صحة البريد الإلكتروني
        let emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
        if (!emailPattern.test(email)) {
            showNotification("Please enter a valid email address", "error");
            return;
        }

        // تعطيل الزر أثناء الإرسال
        verifyBtn.prop('disabled', true);
        verifyBtn.html('<i class="fas fa-spinner fa-spin"></i> Sending...');

        // توليد رقم عشوائي من 6 أرقام
        verificationCode = Math.floor(100000 + Math.random() * 900000);
        console.log("Generated verification code:", verificationCode);

        // إرسال الكود باستخدام EmailJS
        emailjs.send("service_xsbqpga", "template_7k1s8k3", {
            to: email,
            name: "Valued Customer",
            message: `Welcome at Dukkan Waffle Restaurent \n Your verification code is: ${verificationCode}`,
            sendername: "Dukkan Waffle Restaurant"
        })
            .then(() => {
                console.log("Verification code sent successfully");
                showNotification("✓ Verification code sent successfully! Check your email.", "success");
                verifyStatus.html('<div class="verified-badge"><i class="fas fa-check-circle me-1"></i> Code sent to your email</div>');
                verifyBtn.html('<i class="fas fa-redo"></i> Resend');
                verifyBtn.prop('disabled', false);
            })
            .catch((err) => {
                console.error("EmailJS Error:", err);
                showNotification("✗ Failed to send code. Please try again.", "error");
                verifyStatus.html('<div class="error-badge"><i class="fas fa-times-circle me-1"></i> Failed to send code</div>');
                verifyBtn.html('<i class="fas fa-check-circle"></i> Verify');
                verifyBtn.prop('disabled', false);
            });
    });

    // التحقق من العمر عند تغيير تاريخ الميلاد
    $('#Birthday').on('change', function () {
        var birthDate = new Date($(this).val());
        var today = new Date();
        var age = today.getFullYear() - birthDate.getFullYear();
        var monthDiff = today.getMonth() - birthDate.getMonth();

        if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthDate.getDate())) {
            age--;
        }

        if (age < 18) {
            showNotification('You must be at least 18 years old to register.', 'error');
        }
    });

    // معالجة إرسال النموذج
    $('#registerForm').on('submit', function (e) {
        e.preventDefault();
        console.log("Form submitted");

        // التحقق من جميع الحقول
        const fullName = $('#FullName').val().trim();
        const email = $('#Email').val().trim();
        const verificationInput = $('#verificationCode').val().trim();
        const phoneNumber = $('#PhoneNumber').val().trim();
        const password = $('#Password').val();
        const confirmPassword = $('#ConfirmPassword').val();
        const birthday = $('#Birthday').val();

        console.log("Form data:", {
            fullName,
            email,
            verificationInput,
            phoneNumber,
            passwordLength: password.length,
            verificationCode
        });

        // التحقق من الاسم الكامل
        if (fullName.length < 5) {
            showNotification("Full name must be at least 5 characters", "error");
            return;
        }

        // التحقق من البريد الإلكتروني
        let emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
        if (!emailPattern.test(email)) {
            showNotification("Email must be a valid address", "error");
            return;
        }

        // التحقق من رمز التحقق
        if (verificationCode === null) {
            showNotification("Please verify your email first", "error");
            return;
        }

        if (verificationInput != verificationCode) {
            console.log("Invalid verification code - entered:", verificationInput, "expected:", verificationCode);
            showNotification("Invalid verification code", "error");
            return;
        }

        // التحقق من تاريخ الميلاد
        if (!birthday) {
            showNotification("Birthday is required", "error");
            return;
        }

        var birthDate = new Date(birthday);
        var today = new Date();
        var age = today.getFullYear() - birthDate.getFullYear();
        var monthDiff = today.getMonth() - birthDate.getMonth();

        if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthDate.getDate())) {
            age--;
        }

        if (age < 18) {
            showNotification('You must be at least 18 years old to register.', 'error');
            return;
        }

        // التحقق من كلمة المرور
        if (password && password.length < 6) {
            showNotification('Password must be at least 6 characters long', 'error');
            return;
        }

        if (password !== confirmPassword) {
            showNotification('Passwords do not match', 'error');
            return;
        }

        // تعيين حالة التحقق
        $('#isEmailVerified').val('true');
        isEmailVerified = true;

        console.log("All validations passed, submitting form");

        // إرسال النموذج
        showNotification("✓ Registration data validated! Processing...", "success");

        // إرسال النموذج فعلياً
        setTimeout(() => {
            this.submit();
        }, 500);
    });

    // منع إرسال النموذج إذا لم يتم التحقق من البريد الإلكتروني
    $('#submitBtn').on('click', function (e) {
        if (verificationCode === null) {
            e.preventDefault();
            showNotification("Please verify your email first", "error");
            return false;
        }
    });
});