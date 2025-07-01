// Password Toggle Functionality
function togglePassword(inputId, iconId) {
    const passwordInput = document.getElementById(inputId);
    const toggleIcon = document.getElementById(iconId);

    if (passwordInput.type === "password") {
        passwordInput.type = "text";
        toggleIcon.classList.remove("fa-eye");
        toggleIcon.classList.add("fa-eye-slash");
    } else {
        passwordInput.type = "password";
        toggleIcon.classList.remove("fa-eye-slash");
        toggleIcon.classList.add("fa-eye");
    }
}

// Form Validation Functions
function validatePassword(password) {
    return password.length >= 8;
}

function validatePasswordMatch(password, confirmPassword) {
    return password === confirmPassword && password.length > 0;
}

function showError(input, message) {
    input.classList.add('error');
    input.classList.remove('success');
    
    // Remove existing error message
    const existingError = input.parentNode.querySelector('.error-message');
    if (existingError) {
        existingError.remove();
    }
    
    // Add new error message
    const errorDiv = document.createElement('div');
    errorDiv.className = 'error-message';
    errorDiv.textContent = message;
    input.parentNode.appendChild(errorDiv);
}

function showSuccess(input) {
    input.classList.add('success');
    input.classList.remove('error');
    
    // Remove error message
    const existingError = input.parentNode.querySelector('.error-message');
    if (existingError) {
        existingError.remove();
    }
}

function clearValidation(input) {
    input.classList.remove('error', 'success');
    const existingError = input.parentNode.querySelector('.error-message');
    if (existingError) {
        existingError.remove();
    }
}

// Show success message
function showSuccessMessage(message) {
    const form = document.getElementById('createPasswordForm');
    
    // Remove existing success message
    const existingSuccess = document.querySelector('.success-message');
    if (existingSuccess) {
        existingSuccess.remove();
    }
    
    // Create success message
    const successDiv = document.createElement('div');
    successDiv.className = 'success-message';
    successDiv.innerHTML = `<i class="fas fa-check-circle"></i>${message}`;
    
    // Insert before form
    form.parentNode.insertBefore(successDiv, form);
}

// Form Submission
function handleFormSubmit(event) {
    event.preventDefault();
    
    const form = event.target;
    const newPasswordInput = form.querySelector('#newPassword');
    const confirmPasswordInput = form.querySelector('#confirmPassword');
    const submitButton = form.querySelector('.reset-btn');
    
    const newPassword = newPasswordInput.value;
    const confirmPassword = confirmPasswordInput.value;
    
    let isValid = true;
    
    // Clear previous validations
    clearValidation(newPasswordInput);
    clearValidation(confirmPasswordInput);
    
    // Validate new password
    if (!newPassword) {
        showError(newPasswordInput, 'Password is required');
        isValid = false;
    } else if (!validatePassword(newPassword)) {
        showError(newPasswordInput, 'Password must be at least 8 characters');
        isValid = false;
    } else {
        showSuccess(newPasswordInput);
    }
    
    // Validate confirm password
    if (!confirmPassword) {
        showError(confirmPasswordInput, 'Please confirm your password');
        isValid = false;
    } else if (!validatePasswordMatch(newPassword, confirmPassword)) {
        showError(confirmPasswordInput, 'Passwords do not match');
        isValid = false;
    } else {
        showSuccess(confirmPasswordInput);
    }
    
    if (isValid) {
        // Show loading state
        submitButton.classList.add('loading');
        submitButton.disabled = true;
        
        // Simulate API call
        setTimeout(() => {
            // Hide loading state
            submitButton.classList.remove('loading');
            submitButton.disabled = false;
            
            // Handle successful password reset
            console.log('Password reset successful');
            
            // Show success message
            showSuccessMessage('Your password has been reset successfully.');
            
            // Clear form
            newPasswordInput.value = '';
            confirmPasswordInput.value = '';
            clearValidation(newPasswordInput);
            clearValidation(confirmPasswordInput);
            
            // Redirect after delay
            setTimeout(() => {
                window.location.href = 'login.html';
            }, 3000);
            
        }, 2000);
    }
}

// Real-time validation
function setupRealTimeValidation() {
    const newPasswordInput = document.getElementById('newPassword');
    const confirmPasswordInput = document.getElementById('confirmPassword');
    
    // New password validation
    newPasswordInput.addEventListener('blur', function() {
        const password = this.value;
        if (password) {
            if (validatePassword(password)) {
                showSuccess(this);
            } else {
                showError(this, 'Password must be at least 8 characters');
            }
        } else {
            clearValidation(this);
        }
    });
    
    // Confirm password validation
    confirmPasswordInput.addEventListener('blur', function() {
        const password = newPasswordInput.value;
        const confirmPassword = this.value;
        if (confirmPassword) {
            if (validatePasswordMatch(password, confirmPassword)) {
                showSuccess(this);
            } else {
                showError(this, 'Passwords do not match');
            }
        } else {
            clearValidation(this);
        }
    });
    
    // Update confirm password validation when new password changes
    newPasswordInput.addEventListener('input', function() {
        const confirmPassword = confirmPasswordInput.value;
        if (confirmPassword) {
            if (validatePasswordMatch(this.value, confirmPassword)) {
                showSuccess(confirmPasswordInput);
            } else {
                showError(confirmPasswordInput, 'Passwords do not match');
            }
        }
    });
    
    // Clear validation on input
    newPasswordInput.addEventListener('input', function() {
        if (this.classList.contains('error')) {
            clearValidation(this);
        }
    });
    
    confirmPasswordInput.addEventListener('input', function() {
        if (this.classList.contains('error')) {
            clearValidation(this);
        }
    });
}

// Initialize Everything
document.addEventListener("DOMContentLoaded", () => {
    // Setup form submission
    const createPasswordForm = document.getElementById('createPasswordForm');
    if (createPasswordForm) {
        createPasswordForm.addEventListener('submit', handleFormSubmit);
    }
    
    // Setup real-time validation
    setupRealTimeValidation();
    
    // Auto-focus first input
    const newPasswordInput = document.getElementById('newPassword');
    if (newPasswordInput) {
        setTimeout(() => {
            newPasswordInput.focus();
        }, 100);
    }
    
    console.log("Handcrafted create new password functionality initialized!");
});

// Keyboard Navigation
document.addEventListener('keydown', (e) => {
    // Enter key on reset button
    if (e.key === 'Enter' && e.target.classList.contains('reset-btn')) {
        e.target.click();
    }
    
    // Escape key to clear focus
    if (e.key === 'Escape') {
        document.activeElement.blur();
    }
});

// Handle cancel button
document.addEventListener('DOMContentLoaded', () => {
    const cancelButton = document.querySelector('.cancel-btn');
    if (cancelButton) {
        cancelButton.addEventListener('click', () => {
            const confirmCancel = confirm('Are you sure you want to cancel? Any changes will be lost.');
            if (confirmCancel) {
                window.location.href = 'login.html';
            }
        });
    }
});