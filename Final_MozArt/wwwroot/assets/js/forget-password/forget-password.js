// Form Validation Functions
function validateEmail(email) {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
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
    const form = document.getElementById('forgotPasswordForm');
    
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
    const emailInput = form.querySelector('#email');
    const submitButton = form.querySelector('.reset-btn');
    
    const email = emailInput.value.trim();
    
    let isValid = true;
    
    // Clear previous validations
    clearValidation(emailInput);
    
    // Validate email
    if (!email) {
        showError(emailInput, 'Email is required');
        isValid = false;
    } else if (!validateEmail(email)) {
        showError(emailInput, 'Please enter a valid email address');
        isValid = false;
    } else {
        showSuccess(emailInput);
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
            
            // Handle successful password reset request
            console.log('Password reset email sent to:', email);
            
            // Show success message
            showSuccessMessage('Password reset link has been sent to your email address.');
            
            // Clear form
            emailInput.value = '';
            clearValidation(emailInput);
            
            // Optional: Redirect after delay
            // setTimeout(() => {
            //     window.location.href = 'login.html';
            // }, 3000);
            
        }, 2000);
    }
}

// Real-time validation
function setupRealTimeValidation() {
    const emailInput = document.getElementById('email');
    
    // Email validation
    emailInput.addEventListener('blur', function() {
        const email = this.value.trim();
        if (email) {
            if (validateEmail(email)) {
                showSuccess(this);
            } else {
                showError(this, 'Please enter a valid email address');
            }
        } else {
            clearValidation(this);
        }
    });
    
    // Clear validation on input
    emailInput.addEventListener('input', function() {
        if (this.classList.contains('error')) {
            clearValidation(this);
        }
    });
}

// Initialize Everything
document.addEventListener("DOMContentLoaded", () => {
    // Setup form submission
    const forgotPasswordForm = document.getElementById('forgotPasswordForm');
    if (forgotPasswordForm) {
        forgotPasswordForm.addEventListener('submit', handleFormSubmit);
    }
    
    // Setup real-time validation
    setupRealTimeValidation();
    
    console.log("Handcrafted forgot password functionality initialized!");
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

// Auto-focus email input
document.addEventListener('DOMContentLoaded', () => {
    const emailInput = document.getElementById('email');
    if (emailInput) {
        // Focus after a short delay to ensure page is fully loaded
        setTimeout(() => {
            emailInput.focus();
        }, 100);
    }
});

// Handle back navigation
window.addEventListener('popstate', () => {
    // Clear any success messages when navigating back
    const successMessage = document.querySelector('.success-message');
    if (successMessage) {
        successMessage.remove();
    }
});

// Email suggestion feature (optional)
function suggestEmailCorrection(email) {
    const commonDomains = ['gmail.com', 'yahoo.com', 'hotmail.com', 'outlook.com'];
    const emailParts = email.split('@');
    
    if (emailParts.length === 2) {
        const domain = emailParts[1].toLowerCase();
        
        // Check for common typos
        const suggestions = {
            'gmial.com': 'gmail.com',
            'gmai.com': 'gmail.com',
            'yahooo.com': 'yahoo.com',
            'hotmial.com': 'hotmail.com',
            'outlok.com': 'outlook.com'
        };
        
        if (suggestions[domain]) {
            return emailParts[0] + '@' + suggestions[domain];
        }
    }
    
    return null;
}

// Enhanced email validation with suggestions
function enhancedEmailValidation() {
    const emailInput = document.getElementById('email');
    
    emailInput.addEventListener('blur', function() {
        const email = this.value.trim();
        
        if (email && !validateEmail(email)) {
            const suggestion = suggestEmailCorrection(email);
            if (suggestion) {
                const confirmCorrection = confirm(`Did you mean: ${suggestion}?`);
                if (confirmCorrection) {
                    this.value = suggestion;
                    showSuccess(this);
                    return;
                }
            }
            showError(this, 'Please enter a valid email address');
        }
    });
}

// Initialize enhanced email validation
document.addEventListener("DOMContentLoaded", () => {
    enhancedEmailValidation();
});