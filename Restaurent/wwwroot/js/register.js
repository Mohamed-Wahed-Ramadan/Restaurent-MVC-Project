<script src="https://cdn.jsdelivr.net/npm/emailjs-com@3.2.0/dist/email.min.js"></script>
<script>
    // Initialize EmailJS with your Public Key
    emailjs.init('5lHMXpcGuc52IQgst');

    let generatedCode = '';
    let codeExpiryTime = null;

    document.addEventListener('DOMContentLoaded', function () {
        const emailInput = document.getElementById('emailInput');
        const sendVerificationCodeButton = document.getElementById('sendVerificationCode');
        const verificationCodeInput = document.getElementById('verificationCodeInput');
        const registerForm = document.getElementById('registerForm');
        const verificationStatus = document.getElementById('verificationStatus');
        const birthdayInput = document.getElementById('Birthday');
        const passwordInput = document.getElementById('Password');
        const confirmPasswordInput = document.getElementById('ConfirmPassword');

        // إرسال رمز التحقق
        if (sendVerificationCodeButton) {
            sendVerificationCodeButton.addEventListener('click', function () {
                const email = emailInput.value;
                const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

                if (!email) {
                    showVerificationStatus('Please enter your email first.', 'error');
                    return;
                }

                if (!emailRegex.test(email)) {
                    showVerificationStatus('Please enter a valid email address.', 'error');
                    return;
                }

                // Disable button and show loading
                const button = sendVerificationCodeButton;
                const originalText = button.innerHTML;
                button.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Sending...';
                button.disabled = true;

                // Generate a 6-digit random code
                generatedCode = Math.floor(100000 + Math.random() * 900000).toString();
                codeExpiryTime = Date.now() + 10 * 60 * 1000; // 10 minutes

                // Send the code via EmailJS (using your template variables)
                emailjs.send('service_xsbqpga', 'template_8n2d47i', {
                    subject: 'Email Verification Code',
                    to: email, // يروح لحقل {{to}} في القالب
                    sendername: 'Restaurant App',
                    replyto: 'no-reply@restaurantapp.com',
                    name: 'Verification System',
                    message: `Your verification code is: ${generatedCode}`
                }).then(function (response) {
                    showVerificationStatus('Verification code sent to your email!', 'success');
                    verificationCodeInput.focus();

                    // Re-enable button after 30 seconds
                    setTimeout(() => {
                        button.innerHTML = originalText;
                        button.disabled = false;
                    }, 30000);

                }, function (error) {
                    console.error('EmailJS Error:', error);
                    showVerificationStatus('Failed to send verification code. Please try again.', 'error');

                    // Re-enable button on error
                    button.innerHTML = originalText;
                    button.disabled = false;
                });
            });
        }

        // Validate verification code before form submission
        if (registerForm) {
            registerForm.addEventListener('submit', function (e) {
                const enteredCode = verificationCodeInput.value.trim();

                if (codeExpiryTime && Date.now() > codeExpiryTime) {
                    e.preventDefault();
                    showVerificationStatus('Verification code has expired. Please request a new one.', 'error');
                    return false;
                }

                if (enteredCode !== generatedCode) {
                    e.preventDefault();
                    showVerificationStatus('Invalid verification code. Please check your email and try again.', 'error');
                    return false;
                }

                // Validate password
                const password = passwordInput.value;
                const confirmPassword = confirmPasswordInput.value;

                if (password && password.length < 6) {
                    e.preventDefault();
                    showAlert('error', 'Password must be at least 6 characters long');
                    return false;
                }

                if (password !== confirmPassword) {
                    e.preventDefault();
                    showAlert('error', 'Passwords do not match');
                    return false;
                }
            });
        }

        // Real-time check for code
        if (verificationCodeInput) {
            verificationCodeInput.addEventListener('input', function (e) {
                const code = e.target.value;
                if (code.length === 6) {
                    if (code === generatedCode) {
                        showVerificationStatus('Verification code is correct!', 'success');
                    } else {
                        showVerificationStatus('Verification code is incorrect.', 'error');
                    }
                }
            });
        }

        // تحقق من العمر
        if (birthdayInput) {
            birthdayInput.addEventListener('change', function () {
                var birthDate = new Date(this.value);
                var today = new Date();
                var age = today.getFullYear() - birthDate.getFullYear();
                var monthDiff = today.getMonth() - birthDate.getMonth();

                if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthDate.getDate())) {
                    age--;
                }

                if (age < 18) {
                    showAlert('error', 'You must be at least 18 years old to register.');
                }
            });
        }
    });

    function showVerificationStatus(message, type) {
        const statusDiv = document.getElementById('verificationStatus');
        if (statusDiv) {
            statusDiv.innerHTML = `
                <div class="alert alert-${type === 'success' ? 'success' : 'danger'} alert-dismissible fade show" role="alert">
                    <i class="fas ${type === 'success' ? 'fa-check-circle' : 'fa-exclamation-circle'}"></i> 
                    ${message}
                    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                </div>
            `;
        }
    }

    function showAlert(type, message) {
        const existingAlerts = document.querySelectorAll('.alert-dismissible');
        existingAlerts.forEach(alert => alert.remove());

        var alertClass = type === 'success' ? 'alert-success' : 'alert-danger';
        var iconClass = type === 'success' ? 'fa-check-circle' : 'fa-exclamation-circle';

        var alertHtml = '<div class="alert ' + alertClass + ' alert-dismissible fade show mt-3" role="alert">' +
            '<i class="fas ' + iconClass + '"></i> ' +
            message +
            '<button type="button" class="btn-close" data-bs-dismiss="alert"></button>' +
            '</div>';

        const cardBody = document.querySelector('.card-body');
        if (cardBody) {
            cardBody.insertAdjacentHTML('afterbegin', alertHtml);
        }

        setTimeout(function () {
            const alertElement = document.querySelector('.alert-dismissible');
            if (alertElement) {
                alertElement.style.transition = 'opacity 0.3s';
                alertElement.style.opacity = '0';
                setTimeout(() => {
                    if (alertElement.parentNode) {
                        alertElement.parentNode.removeChild(alertElement);
                    }
                }, 300);
            }
        }, 5000);
    }
</script>
