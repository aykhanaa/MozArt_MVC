// document.addEventListener("DOMContentLoaded", () => {
//     const userAccountBtn = document.getElementById("userAccountBtn")
//     const userDropdown = document.getElementById("userDropdown")
//     let isDropdownOpen = false
  
//     if (userAccountBtn && userDropdown) {
//       userAccountBtn.addEventListener("click", (e) => {
//         e.preventDefault()
//         e.stopPropagation()
  
//         isDropdownOpen = !isDropdownOpen
  
//         if (isDropdownOpen) {
//           userDropdown.classList.add("show")
//           userAccountBtn.setAttribute("aria-expanded", "true")
//         } else {
//           userDropdown.classList.remove("show")
//           userAccountBtn.setAttribute("aria-expanded", "false")
//         }
//       })
  
//       document.addEventListener("click", (e) => {
//         if (!e.target.closest(".user-account-dropdown")) {
//           userDropdown.classList.remove("show")
//           userAccountBtn.setAttribute("aria-expanded", "false")
//           isDropdownOpen = false
//         }
//       })
  
//       userAccountBtn.addEventListener("keydown", (e) => {
//         if (e.key === "Enter" || e.key === " ") {
//           e.preventDefault()
//           userAccountBtn.click()
//         }
  
//         if (e.key === "Escape") {
//           userDropdown.classList.remove("show")
//           userAccountBtn.setAttribute("aria-expanded", "false")
//           isDropdownOpen = false
//           userAccountBtn.focus()
//         }
//       })
  
//       const dropdownItems = userDropdown.querySelectorAll(".dropdown-item")
  
//       dropdownItems.forEach((item, index) => {
//         item.addEventListener("keydown", (e) => {
//           if (e.key === "ArrowDown") {
//             e.preventDefault()
//             const nextItem = dropdownItems[index + 1] || dropdownItems[0]
//             nextItem.focus()
//           }
  
//           if (e.key === "ArrowUp") {
//             e.preventDefault()
//             const prevItem = dropdownItems[index - 1] || dropdownItems[dropdownItems.length - 1]
//             prevItem.focus()
//           }
  
//           if (e.key === "Escape") {
//             userDropdown.classList.remove("show")
//             userAccountBtn.setAttribute("aria-expanded", "false")
//             isDropdownOpen = false
//             userAccountBtn.focus()
//           }
//         })
//       })
  
//       userAccountBtn.setAttribute("aria-haspopup", "true")
//       userAccountBtn.setAttribute("aria-expanded", "false")
//     }
  
//     const mobileMenuToggle = document.querySelector(".fa-bars")
//     const mobileMenu = document.querySelector(".responsive-menu #menu")
  
//     if (mobileMenuToggle && mobileMenu) {
//       mobileMenuToggle.addEventListener("click", () => {
//         mobileMenu.classList.toggle("active")
//       })
//     }
  
//     const anchorLinks = document.querySelectorAll('a[href^="#"]')
  
//     anchorLinks.forEach((link) => {
//       link.addEventListener("click", (e) => {
//         const href = link.getAttribute("href")
  
//         if (href !== "#") {
//           const target = document.querySelector(href)
  
//           if (target) {
//             e.preventDefault()
//             target.scrollIntoView({
//               behavior: "smooth",
//               block: "start",
//             })
//           }
//         }
//       })
//     })
  
//     console.log(" Header user account functionality initialized!")
//   })
  
//   window.checkUserLogin = () => {
    
//     return false
//   }
  
//   window.updateUserDropdown = (userData) => {
//     const userDropdown = document.getElementById("userDropdown")
  
//     if (userDropdown && userData) {
//       userDropdown.innerHTML = `
//         <a href="profile.html" class="dropdown-item">
//           <i class="fas fa-user"></i> Profile
//         </a>
//         <a href="my-reservations.html" class="dropdown-item">
//           <i class="fas fa-calendar-alt"></i> My Reservations
//         </a>
//         <a href="settings.html" class="dropdown-item">
//           <i class="fas fa-cog"></i> Settings
//         </a>
//         <div style="border-top: 1px solid rgba(228, 204, 180, 0.2); margin: 8px 0;"></div>
//         <a href="#" class="dropdown-item" onclick="logout()">
//           <i class="fas fa-sign-out-alt"></i> Logout
//         </a>
//       `
//     }
//   }
  
//   window.logout = () => {
//     localStorage.removeItem("userData")
//     sessionStorage.removeItem("userToken")
//     window.location.href = "home.html"
//   }


//   function togglePassword() {
//     const passwordInput = document.getElementById("passwordInput");
//     const toggleIcon = document.getElementById("toggleIcon");

//     if (passwordInput.type === "password") {
//         passwordInput.type = "text";
//         toggleIcon.classList.remove("fa-eye");
//         toggleIcon.classList.add("fa-eye-slash");
//     } else {
//         passwordInput.type = "password";
//         toggleIcon.classList.remove("fa-eye-slash");
//         toggleIcon.classList.add("fa-eye");
//     }
// }




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
function validateFullName(fullname) {
  return fullname.trim().length >= 2 && /^[a-zA-Z\s]+$/.test(fullname.trim());
}

function validateUsername(username) {
  const usernameRegex = /^[a-zA-Z0-9_]{3,20}$/;
  return usernameRegex.test(username);
}

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

// Form Submission
function handleFormSubmit(event) {
  event.preventDefault();
  
  const form = event.target;
  const fullnameInput = form.querySelector('#fullname');
  const usernameInput = form.querySelector('#username');
  const emailInput = form.querySelector('#email');
  const passwordInput = form.querySelector('#passwordInput');
  const confirmPasswordInput = form.querySelector('#confirmPasswordInput');
  const termsCheckbox = form.querySelector('#terms');
  const submitButton = form.querySelector('.register-btn');
  
  const fullname = fullnameInput.value.trim();
  const username = usernameInput.value.trim();
  const email = emailInput.value.trim();
  const password = passwordInput.value;
  const confirmPassword = confirmPasswordInput.value;
  const termsAccepted = termsCheckbox.checked;
  
  let isValid = true;
  
  // Clear previous validations
  [fullnameInput, usernameInput, emailInput].forEach(clearValidation);
  
  // Validate full name
  if (!fullname) {
      showError(fullnameInput, 'Full name is required');
      isValid = false;
  } else if (!validateFullName(fullname)) {
      showError(fullnameInput, 'Please enter a valid full name (letters only)');
      isValid = false;
  } else {
      showSuccess(fullnameInput);
  }
  
  // Validate username
  if (!username) {
      showError(usernameInput, 'Username is required');
      isValid = false;
  } else if (!validateUsername(username)) {
      showError(usernameInput, 'Username must be 3-20 characters (letters, numbers, underscore only)');
      isValid = false;
  } else {
      showSuccess(usernameInput);
  }
  
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
  
  // Validate terms acceptance
  if (!termsAccepted) {
      alert('Please accept the Terms & Conditions to continue');
      isValid = false;
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
          
          // Handle successful registration
          console.log('Registration successful:', {
              fullname: fullname,
              username: username,
              email: email,
              password: password
          });
          
          alert('Registration successful! (This is a demo)');
          
          // Redirect to login page
          // window.location.href = 'login.html';
      }, 2000);
  }
}

// Real-time validation
function setupRealTimeValidation() {
  const fullnameInput = document.getElementById('fullname');
  const usernameInput = document.getElementById('username');
  const emailInput = document.getElementById('email');
  
  // Full name validation
  fullnameInput.addEventListener('blur', function() {
      const fullname = this.value.trim();
      if (fullname) {
          if (validateFullName(fullname)) {
              showSuccess(this);
          } else {
              showError(this, 'Please enter a valid full name (letters only)');
          }
      } else {
          clearValidation(this);
      }
  });
  
  // Username validation
  usernameInput.addEventListener('blur', function() {
      const username = this.value.trim();
      if (username) {
          if (validateUsername(username)) {
              showSuccess(this);
          } else {
              showError(this, 'Username must be 3-20 characters (letters, numbers, underscore only)');
          }
      } else {
          clearValidation(this);
      }
  });
  
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
  [fullnameInput, usernameInput, emailInput].forEach(input => {
      input.addEventListener('input', function() {
          if (this.classList.contains('error')) {
              clearValidation(this);
          }
      });
  });
}

// Initialize Everything
document.addEventListener("DOMContentLoaded", () => {
  // Setup form submission
  const registerForm = document.getElementById('registerForm');
  if (registerForm) {
      registerForm.addEventListener('submit', handleFormSubmit);
  }
  
  // Setup real-time validation
  setupRealTimeValidation();
  
  console.log("Handcrafted register functionality initialized!");
});

// Keyboard Navigation
document.addEventListener('keydown', (e) => {
  // Enter key on register button
  if (e.key === 'Enter' && e.target.classList.contains('register-btn')) {
      e.target.click();
  }
  
  // Escape key to clear focus
  if (e.key === 'Escape') {
      document.activeElement.blur();
  }
});

// Username availability check (simulation)
function checkUsernameAvailability(username) {
  // Simulate API call
  const unavailableUsernames = ['admin', 'user', 'test', 'demo'];
  return !unavailableUsernames.includes(username.toLowerCase());
}

// Enhanced username validation with availability check
function enhancedUsernameValidation() {
  const usernameInput = document.getElementById('username');
  let timeoutId;
  
  usernameInput.addEventListener('input', function() {
      clearTimeout(timeoutId);
      const username = this.value.trim();
      
      if (username.length >= 3) {
          timeoutId = setTimeout(() => {
              if (validateUsername(username)) {
                  if (checkUsernameAvailability(username)) {
                      showSuccess(this);
                  } else {
                      showError(this, 'Username is already taken');
                  }
              }
          }, 500);
      }
  });
}

// Initialize enhanced username validation
document.addEventListener("DOMContentLoaded", () => {
  enhancedUsernameValidation();
});