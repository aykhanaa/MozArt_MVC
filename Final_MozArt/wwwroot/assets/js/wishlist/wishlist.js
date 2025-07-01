document.addEventListener("DOMContentLoaded", () => {
    const userAccountBtn = document.getElementById("userAccountBtn")
    const userDropdown = document.getElementById("userDropdown")
    let isDropdownOpen = false
  
    if (userAccountBtn && userDropdown) {
      userAccountBtn.addEventListener("click", (e) => {
        e.preventDefault()
        e.stopPropagation()
  
        isDropdownOpen = !isDropdownOpen
  
        if (isDropdownOpen) {
          userDropdown.classList.add("show")
          userAccountBtn.setAttribute("aria-expanded", "true")
        } else {
          userDropdown.classList.remove("show")
          userAccountBtn.setAttribute("aria-expanded", "false")
        }
      })
  
      document.addEventListener("click", (e) => {
        if (!e.target.closest(".user-account-dropdown")) {
          userDropdown.classList.remove("show")
          userAccountBtn.setAttribute("aria-expanded", "false")
          isDropdownOpen = false
        }
      })
  
      userAccountBtn.addEventListener("keydown", (e) => {
        if (e.key === "Enter" || e.key === " ") {
          e.preventDefault()
          userAccountBtn.click()
        }
  
        if (e.key === "Escape") {
          userDropdown.classList.remove("show")
          userAccountBtn.setAttribute("aria-expanded", "false")
          isDropdownOpen = false
          userAccountBtn.focus()
        }
      })
  
      const dropdownItems = userDropdown.querySelectorAll(".dropdown-item")
  
      dropdownItems.forEach((item, index) => {
        item.addEventListener("keydown", (e) => {
          if (e.key === "ArrowDown") {
            e.preventDefault()
            const nextItem = dropdownItems[index + 1] || dropdownItems[0]
            nextItem.focus()
          }
  
          if (e.key === "ArrowUp") {
            e.preventDefault()
            const prevItem = dropdownItems[index - 1] || dropdownItems[dropdownItems.length - 1]
            prevItem.focus()
          }
  
          if (e.key === "Escape") {
            userDropdown.classList.remove("show")
            userAccountBtn.setAttribute("aria-expanded", "false")
            isDropdownOpen = false
            userAccountBtn.focus()
          }
        })
      })
  
      userAccountBtn.setAttribute("aria-haspopup", "true")
      userAccountBtn.setAttribute("aria-expanded", "false")
    }
  
    const mobileMenuToggle = document.querySelector(".fa-bars")
    const mobileMenu = document.querySelector(".responsive-menu #menu")
  
    if (mobileMenuToggle && mobileMenu) {
      mobileMenuToggle.addEventListener("click", () => {
        mobileMenu.classList.toggle("active")
      })
    }
  
    const anchorLinks = document.querySelectorAll('a[href^="#"]')
  
    anchorLinks.forEach((link) => {
      link.addEventListener("click", (e) => {
        const href = link.getAttribute("href")
  
        if (href !== "#") {
          const target = document.querySelector(href)
  
          if (target) {
            e.preventDefault()
            target.scrollIntoView({
              behavior: "smooth",
              block: "start",
            })
          }
        }
      })
    })
  
    console.log(" Header user account functionality initialized!")
  })
  
  window.checkUserLogin = () => {
    
    return false
  }
  
  window.updateUserDropdown = (userData) => {
    const userDropdown = document.getElementById("userDropdown")
  
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
      `
    }
  }
  
  window.logout = () => {
    localStorage.removeItem("userData")
    sessionStorage.removeItem("userToken")
    window.location.href = "home.html"
  }


  document.addEventListener('DOMContentLoaded', function() {
    // Scroll to top functionality
        // This is a simplified version of the $.scrollUp plugin
        const createScrollToTop = () => {
            // Create the button element
            const scrollButton = document.createElement('div');
            scrollButton.id = 'scrollUp';
            scrollButton.innerHTML = '<i class="fas fa-long-arrow-alt-up"></i>';
            scrollButton.style.display = 'none';
            scrollButton.style.position = 'fixed';
            scrollButton.style.zIndex = '2147483647';
            scrollButton.style.bottom = '20px';
            scrollButton.style.right = '20px';
            scrollButton.style.cursor = 'pointer';
            document.body.appendChild(scrollButton);
    
            // Show/hide based on scroll position
            window.addEventListener('scroll', function() {
                if (window.pageYOffset > 100) {
                    scrollButton.style.display = 'block';
                } else {
                    scrollButton.style.display = 'none';
                }
            });
    
            // Scroll to top when clicked
            scrollButton.addEventListener('click', function() {
                window.scrollTo({
                    top: 0,
                    behavior: 'smooth'
                });
            });
        };
    
        createScrollToTop();
    });