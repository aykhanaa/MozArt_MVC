
// Password Toggle Functionality
function togglePassword() {
  const passwordInput = document.getElementById("passwordInput");
  const toggleIcon = document.getElementById("toggleIcon");

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

// Form Validation
function validateEmail(email) {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  return emailRegex.test(email);
}

function validatePassword(password) {
  return password.length >= 6;
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

// Form Submission
function handleFormSubmit(event) {
  event.preventDefault();
  
  const form = event.target;
  const emailInput = form.querySelector('#email');
  const passwordInput = form.querySelector('#passwordInput');
  const submitButton = form.querySelector('.login-btn');
  const rememberMe = form.querySelector('#remember-me').checked;
  
  const email = emailInput.value.trim();
  const password = passwordInput.value;
  
  let isValid = true;
  
  // Clear previous validations
  clearValidation(emailInput);
  clearValidation(passwordInput);
  
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
  
  // Validate password
  if (!password) {
      showError(passwordInput, 'Password is required');
      isValid = false;
  } else if (!validatePassword(password)) {
      showError(passwordInput, 'Password must be at least 6 characters');
      isValid = false;
  } else {
      showSuccess(passwordInput);
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
          
          // Handle successful login
          console.log('Login successful:', {
              email: email,
              password: password,
              rememberMe: rememberMe
          });
          
          // Store user data if remember me is checked
          if (rememberMe) {
              localStorage.setItem('rememberedEmail', email);
          } else {
              localStorage.removeItem('rememberedEmail');
          }
          
          // Redirect to dashboard or home page
          // window.location.href = 'dashboard.html';
          
          alert('Login successful! (This is a demo)');
      }, 2000);
  }
}

// Real-time validation
function setupRealTimeValidation() {
  const emailInput = document.getElementById('email');
  const passwordInput = document.getElementById('passwordInput');
  
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
  
  passwordInput.addEventListener('blur', function() {
      const password = this.value;
      if (password) {
          if (validatePassword(password)) {
              showSuccess(this);
          } else {
              showError(this, 'Password must be at least 6 characters');
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
  
  passwordInput.addEventListener('input', function() {
      if (this.classList.contains('error')) {
          clearValidation(this);
      }
  });
}

// Remember Me Functionality
function loadRememberedEmail() {
  const rememberedEmail = localStorage.getItem('rememberedEmail');
  if (rememberedEmail) {
      document.getElementById('email').value = rememberedEmail;
      document.getElementById('remember-me').checked = true;
  }
}

// User Account Dropdown (from original code)
function initializeUserDropdown() {
  const userAccountBtn = document.getElementById("userAccountBtn");
  const userDropdown = document.getElementById("userDropdown");
  let isDropdownOpen = false;

  if (userAccountBtn && userDropdown) {
      userAccountBtn.addEventListener("click", (e) => {
          e.preventDefault();
          e.stopPropagation();

          isDropdownOpen = !isDropdownOpen;

          if (isDropdownOpen) {
              userDropdown.classList.add("show");
              userAccountBtn.setAttribute("aria-expanded", "true");
          } else {
              userDropdown.classList.remove("show");
              userAccountBtn.setAttribute("aria-expanded", "false");
          }
      });

      document.addEventListener("click", (e) => {
          if (!e.target.closest(".user-account-dropdown")) {
              userDropdown.classList.remove("show");
              userAccountBtn.setAttribute("aria-expanded", "false");
              isDropdownOpen = false;
          }
      });

      userAccountBtn.addEventListener("keydown", (e) => {
          if (e.key === "Enter" || e.key === " ") {
              e.preventDefault();
              userAccountBtn.click();
          }

          if (e.key === "Escape") {
              userDropdown.classList.remove("show");
              userAccountBtn.setAttribute("aria-expanded", "false");
              isDropdownOpen = false;
              userAccountBtn.focus();
          }
      });

      const dropdownItems = userDropdown.querySelectorAll(".dropdown-item");

      dropdownItems.forEach((item, index) => {
          item.addEventListener("keydown", (e) => {
              if (e.key === "ArrowDown") {
                  e.preventDefault();
                  const nextItem = dropdownItems[index + 1] || dropdownItems[0];
                  nextItem.focus();
              }

              if (e.key === "ArrowUp") {
                  e.preventDefault();
                  const prevItem = dropdownItems[index - 1] || dropdownItems[dropdownItems.length - 1];
                  prevItem.focus();
              }

              if (e.key === "Escape") {
                  userDropdown.classList.remove("show");
                  userAccountBtn.setAttribute("aria-expanded", "false");
                  isDropdownOpen = false;
                  userAccountBtn.focus();
              }
          });
      });

      userAccountBtn.setAttribute("aria-haspopup", "true");
      userAccountBtn.setAttribute("aria-expanded", "false");
  }
}

// Mobile Menu Toggle
function initializeMobileMenu() {
  const mobileMenuToggle = document.querySelector(".fa-bars");
  const mobileMenu = document.querySelector(".responsive-menu #menu");

  if (mobileMenuToggle && mobileMenu) {
      mobileMenuToggle.addEventListener("click", () => {
          mobileMenu.classList.toggle("active");
      });
  }
}

// Smooth Scrolling for Anchor Links
function initializeSmoothScrolling() {
  const anchorLinks = document.querySelectorAll('a[href^="#"]');

  anchorLinks.forEach((link) => {
      link.addEventListener("click", (e) => {
          const href = link.getAttribute("href");

          if (href !== "#") {
              const target = document.querySelector(href);

              if (target) {
                  e.preventDefault();
                  target.scrollIntoView({
                      behavior: "smooth",
                      block: "start",
                  });
              }
          }
      });
  });
}

// User Login Check
window.checkUserLogin = () => {
  return false;
};

// Update User Dropdown
window.updateUserDropdown = (userData) => {
  const userDropdown = document.getElementById("userDropdown");

  if (userDropdown && userData) {
      userDropdown.innerHTML = `
          <a href="profile.html" class="dropdown-item">
              <i class="fas fa-user"></i> Profile
          </a>
          <a href="my-reservations.html" class="dropdown-item">
              <i class="fas fa-calendar-alt"></i> My Reservations
          </a>
          <a href="settings.html" class="dropdown-item">
              <i class="fas fa-cog"></i> Settings
          </a>
          <div style="border-top: 1px solid rgba(228, 204, 180, 0.2); margin: 8px 0;"></div>
          <a href="#" class="dropdown-item" onclick="logout()">
              <i class="fas fa-sign-out-alt"></i> Logout
          </a>
      `;
  }
};

// Logout Function
window.logout = () => {
  localStorage.removeItem("userData");
  sessionStorage.removeItem("userToken");
  localStorage.removeItem("rememberedEmail");
  window.location.href = "home.html";
};

// Initialize Everything
document.addEventListener("DOMContentLoaded", () => {
  // Setup form submission
  const loginForm = document.getElementById('loginForm');
  if (loginForm) {
      loginForm.addEventListener('submit', handleFormSubmit);
  }
  
  // Setup real-time validation
  setupRealTimeValidation();
  
  // Load remembered email
  loadRememberedEmail();
  
  // Initialize other functionalities
  initializeUserDropdown();
  initializeMobileMenu();
  initializeSmoothScrolling();
  
  console.log("Handcrafted login functionality initialized!");
});

// Keyboard Navigation
document.addEventListener('keydown', (e) => {
  // Enter key on login button
  if (e.key === 'Enter' && e.target.classList.contains('login-btn')) {
      e.target.click();
  }
  
  // Escape key to clear focus
  if (e.key === 'Escape') {
      document.activeElement.blur();
  }
});

// Form Auto-save (Optional)
function autoSaveForm() {
  const emailInput = document.getElementById('email');
  const rememberCheckbox = document.getElementById('remember-me');
  
  if (emailInput && rememberCheckbox) {
      emailInput.addEventListener('input', () => {
          if (rememberCheckbox.checked) {
              localStorage.setItem('draftEmail', emailInput.value);
          }
      });
      
      // Load draft email
      const draftEmail = localStorage.getItem('draftEmail');
      if (draftEmail && !emailInput.value) {
          emailInput.value = draftEmail;
      }
  }
}

// Initialize auto-save
document.addEventListener('DOMContentLoaded', autoSaveForm);

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