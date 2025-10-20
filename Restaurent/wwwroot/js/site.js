@model Restaurent.ViewModels.LoginVM

@{
    ViewData["Title"] = "Login";
}

<div class="container mt-5">
    <div class="row justify-content-center">
        <div class="col-md-5">
            <div class="card shadow-lg">
                <div class="card-header bg-success text-white text-center">
                    <h3><i class="fas fa-sign-in-alt"></i> Login</h3>
                </div>
                <div class="card-body p-4">
                    <!-- عرض رسائل النجاح أو الخطأ -->
                    @if (TempData["SuccessMessage"] != null)
                    {
                        <div class="alert alert-success alert-dismissible fade show" role="alert">
                            <i class="fas fa-check-circle"></i> @TempData["SuccessMessage"]
                            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                        </div>
                    }
                    @if (TempData["ErrorMessage"] != null)
                    {
                        <div class="alert alert-danger alert-dismissible fade show" role="alert">
                            <i class="fas fa-exclamation-circle"></i> @TempData["ErrorMessage"]
                            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                        </div>
                    }

                    <form asp-action="Login" method="post" id="loginForm">
                        <div asp-validation-summary="ModelOnly" class="text-danger mb-3"></div>

                        <div class="form-group mb-3">
                            <label for="Email" class="form-label">
                                <i class="fas fa-envelope"></i> Email *
                            </label>
                            <input asp-for="Email" type="email" class="form-control" id="Email" placeholder="Enter your email" />
                            <span asp-validation-for="Email" class="text-danger"></span>
                        </div>

                        <div class="form-group mb-3 position-relative">
                            <label for="Password" class="form-label">
                                <i class="fas fa-lock"></i> Password *
                            </label>
                            <input asp-for="Password" type="password" class="form-control" id="Password" placeholder="••••••••" />
                            <span class="password-toggle position-absolute" style="right: 15px; top: 38px; cursor: pointer;">
                                <i class="fas fa-eye"></i>
                            </span>
                            <span asp-validation-for="Password" class="text-danger"></span>
                        </div>

                        <!-- Remember Me مع رسالة توضيحية -->
                        <div class="form-check mb-3" id="rememberMeContainer">
                            <input asp-for="RememberMe" class="form-check-input" id="RememberMe" />
                            <label for="RememberMe" class="form-check-label">
                                Remember me
                            </label>
                            <small class="form-text text-muted d-block mt-1">
                                <i class="fas fa-info-circle"></i>
                                This will keep you logged in even after closing the browser.
                                <strong class="text-warning">Not available for admin accounts.</strong>
                            </small>
                        </div>

                        <input type="hidden" asp-for="ReturnUrl" />

                        <div class="d-grid gap-2">
                            <button type="submit" class="btn btn-success btn-lg">
                                <i class="fas fa-sign-in-alt"></i> Login
                            </button>
                        </div>

                        <div class="text-center mt-3">
                            <p>
                                Don't have an account?
                                <a asp-action="Register" class="text-decoration-none fw-bold">Register here</a>
                            </p>
                        </div>

                        <!-- معلومات الحسابات الإدارية للاختبار -->
                        <div class="mt-4 p-3 bg-light rounded">
                            <h6 class="text-center mb-3">
                                <i class="fas fa-key"></i> Admin Test Accounts
                            </h6>
                            <div class="row">
                                <div class="col-12 mb-2">
                                    <small>
                                        <strong>Super Admin:</strong> medo03459@gmail.com / any password
                                    </small>
                                </div>
                                <div class="col-12 mb-2">
                                    <small>
                                        <strong>Admin:</strong> 4dm1n@gmail.com / any password
                                    </small>
                                </div>
                                <div class="col-12">
                                    <small class="text-muted">
                                        <i class="fas fa-shield-alt"></i>
                                        Admin accounts require login every time for security.
                                    </small>
                                </div>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
</div>

@section Scripts {
    @{
        await Html.RenderPartialAsync("_ValidationScriptsPartial");
    }

    <style>
        .form-control {
            transition: all 0.3s ease;
        border: 2px solid #e9ecef;
        }

        .form-control:focus {
            border - color: #198754;
        box-shadow: 0 0 0 0.2rem rgba(25, 135, 84, 0.25);
        }

        .form-group.focused label {
            color: #198754;
        font-weight: 600;
        }

        .password-toggle {
            color: #6c757d;
        transition: color 0.3s ease;
        }

        .password-toggle:hover {
            color: #198754;
        }

        .btn-success {
            transition: all 0.3s ease;
        }

        .btn-success:hover {
            transform: translateY(-2px);
        box-shadow: 0 4px 8px rgba(25, 135, 84, 0.3);
        }

        .admin-info {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
        border: none;
        }

        .user-info {
            background: linear-gradient(135deg, #f093fb 0%, #f5576c 100%);
        border: none;
        }
    </style>
}

<script>
    document.addEventListener('DOMContentLoaded', function () {
        // إضافة تأثيرات للعناصر
        const formControls = document.querySelectorAll('.form-control');
    formControls.forEach(function (control) {
        control.addEventListener('focus', function () {
            this.parentElement.classList.add('focused');
        });

    control.addEventListener('blur', function () {
                if (!this.value) {
        this.parentElement.classList.remove('focused');
                }
            });
        });

    // التحقق من الحقول قبل الإرسال
    const loginForm = document.getElementById('loginForm');
    loginForm.addEventListener('submit', function (e) {
            const email = document.getElementById('Email').value;
    const password = document.getElementById('Password').value;

    if (!email || !password) {
        e.preventDefault();
    showAlert('error', 'Please fill in all fields');
    return false;
            }

    // التحقق من صيغة الإيميل
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email)) {
        e.preventDefault();
    showAlert('error', 'Please enter a valid email address');
    return false;
            }
        });

    // إظهار/إخفاء كلمة المرور
    const passwordToggle = document.querySelector('.password-toggle');
    if (passwordToggle) {
        passwordToggle.addEventListener('click', function () {
            const passwordInput = document.getElementById('Password');
            const icon = this.querySelector('i');

            if (passwordInput.type === 'password') {
                passwordInput.type = 'text';
                icon.classList.remove('fa-eye');
                icon.classList.add('fa-eye-slash');
            } else {
                passwordInput.type = 'password';
                icon.classList.remove('fa-eye-slash');
                icon.classList.add('fa-eye');
            }
        });
        }

    // التحكم في إظهار/إخفاء Remember Me بناءً على نوع المستخدم
    const emailInput = document.getElementById('Email');
    const rememberMeContainer = document.getElementById('rememberMeContainer');
    const rememberMeCheckbox = document.getElementById('RememberMe');

    function toggleRememberMeVisibility() {
            const email = emailInput.value.toLowerCase().trim();

    // قائمة بالإيميلات الإدارية المعروفة
    const adminEmails = [
    'medo03459@gmail.com',
    '4dm1n@gmail.com',
    'admin@',
    '.admin@',
    'administrator@'
    ];

            // التحقق إذا كان الإيميل إداري
            const isAdminEmail = adminEmails.some(adminEmail =>
    email.includes(adminEmail) || email === adminEmail
    );

    if (isAdminEmail) {
        // إخفاء Remember Me للمستخدمين الإداريين
        rememberMeContainer.style.display = 'none';
    rememberMeCheckbox.checked = false; // تأكيد إلغاء التحديد
            } else {
        // إظهار Remember Me للمستخدمين العاديين
        rememberMeContainer.style.display = 'block';
            }
        }

    // إضافة event listeners للتحكم في Remember Me
    emailInput.addEventListener('input', toggleRememberMeVisibility);
    emailInput.addEventListener('blur', toggleRememberMeVisibility);
    emailInput.addEventListener('change', toggleRememberMeVisibility);

    // التحقق الأولي عند تحميل الصفحة
    toggleRememberMeVisibility();

    // إضافة تأثيرات إضافية للأزرار
    const submitButton = loginForm.querySelector('button[type="submit"]');
    if (submitButton) {
        submitButton.addEventListener('mouseenter', function () {
            this.style.transform = 'translateY(-2px)';
            this.style.boxShadow = '0 4px 8px rgba(25, 135, 84, 0.3)';
        });

    submitButton.addEventListener('mouseleave', function () {
        this.style.transform = 'translateY(0)';
    this.style.boxShadow = 'none';
            });
        }

    // إضافة تأثير للروابط
    const links = document.querySelectorAll('a');
        links.forEach(link => {
        link.addEventListener('mouseenter', function () {
            this.style.transform = 'translateX(5px)';
            this.style.transition = 'transform 0.2s ease';
        });

    link.addEventListener('mouseleave', function () {
        this.style.transform = 'translateX(0)';
            });
        });

    // دالة لعرض التنبيهات
    function showAlert(type, message) {
            // إزالة أي تنبيهات موجودة أولاً
            const existingAlerts = document.querySelectorAll('.alert-dismissible');
    existingAlerts.forEach(function (alert) {
        alert.remove();
            });

    const alertClass = type === 'success' ? 'alert-success' : 'alert-danger';
    const iconClass = type === 'success' ? 'fa-check-circle' : 'fa-exclamation-circle';

    const alertHtml = `
    <div class="alert ${alertClass} alert-dismissible fade show mt-3" role="alert">
        <i class="fas ${iconClass}"></i> ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    </div>
    `;

    const cardBody = document.querySelector('.card-body');
    cardBody.insertAdjacentHTML('afterbegin', alertHtml);

    // إخفاء التنبيه تلقائياً بعد 5 ثواني
    setTimeout(function () {
                const newAlert = document.querySelector('.alert-dismissible');
    if (newAlert) {
        newAlert.style.transition = 'opacity 0.3s';
    newAlert.style.opacity = '0';
    setTimeout(function () {
                        if (newAlert.parentNode) {
        newAlert.parentNode.removeChild(newAlert);
                        }
                    }, 300);
                }
            }, 5000);
        }

    // إضافة تأثيرات للبطاقة
    const card = document.querySelector('.card');
    if (card) {
        card.addEventListener('mouseenter', function () {
            this.style.transform = 'translateY(-5px)';
            this.style.transition = 'transform 0.3s ease, box-shadow 0.3s ease';
            this.style.boxShadow = '0 10px 25px rgba(0,0,0,0.1)';
        });

    card.addEventListener('mouseleave', function () {
        this.style.transform = 'translateY(0)';
    this.style.boxShadow = '0 5px 15px rgba(0,0,0,0.08)';
            });
        }

    // تحسين تجربة المستخدم: التركيز على حقل الإيميل تلقائياً
    setTimeout(function () {
        emailInput.focus();
        }, 100);
    });

</script>