"use strict";

document.addEventListener('DOMContentLoaded', function() {
    const body = document.body;

    // Background image and color handling
    document.querySelectorAll('[data-bg-image]').forEach(function(element) {
        const image = element.getAttribute('data-bg-image');
        element.style.backgroundImage = `url(${image})`;
    });

    document.querySelectorAll('[data-bg-color]').forEach(function(element) {
        const color = element.getAttribute('data-bg-color');
        element.style.backgroundColor = color;
    });

    // Sticky header
    window.addEventListener('scroll', function() {
        const stickyHeaders = document.querySelectorAll('.sticky-header');
        if (window.scrollY > 350) {
            stickyHeaders.forEach(header => header.classList.add('is-sticky'));
        } else {
            stickyHeaders.forEach(header => header.classList.remove('is-sticky'));
        }
    });

    // Submenu and mega menu alignment
    const subMenuMegaMenuAlignment = () => {
        const siteMainMenus = document.querySelectorAll('.site-main-menu');
        let thisElement;

        siteMainMenus.forEach(function(menu) {
            thisElement = menu;
            if ((thisElement.classList.contains('site-main-menu-left') || thisElement.classList.contains('site-main-menu-right')) && 
                thisElement.closest('.section-fluid')) {
                const megaMenu = thisElement.querySelector('.mega-menu');
                thisElement.style.position = "relative";
                
                if (thisElement.classList.contains('site-main-menu-left')) {
                    if (megaMenu) {
                        megaMenu.style.left = "0px";
                        megaMenu.style.right = "auto";
                    }
                } else if (thisElement.classList.contains('site-main-menu-left')) {
                    if (megaMenu) {
                        megaMenu.style.right = "0px";
                        megaMenu.style.left = "auto";
                    }
                }
            }
        });

        const subMenus = document.querySelectorAll('.sub-menu');
        if (subMenus.length) {
            subMenus.forEach(function(subMenu) {
                thisElement = subMenu;
                const elementRect = thisElement.getBoundingClientRect();
                const elementOffsetLeft = elementRect.left + window.scrollX;
                const elementWidth = thisElement.offsetWidth;
                const windowWidth = window.innerWidth - 10;
                const isElementVisible = (elementOffsetLeft + elementWidth < windowWidth);
                
                if (!isElementVisible) {
                    if (thisElement.classList.contains('mega-menu')) {
                        const parentRect = thisElement.parentElement.getBoundingClientRect();
                        const thisOffsetLeft = parentRect.left + window.scrollX;
                        const widthDiff = windowWidth - elementWidth;
                        const leftPos = thisOffsetLeft - (widthDiff / 2);
                        
                        thisElement.style.left = -leftPos + "px";
                        thisElement.style.setProperty("left", -leftPos + "px", "important");
                        thisElement.parentElement.style.position = "relative";
                    } else {
                        thisElement.parentElement.classList.add('align-left');
                    }
                } else {
                    thisElement.removeAttribute('style');
                    thisElement.parentElement.classList.remove('align-left');
                }
            });
        }
    };

    // Call the function initially
    subMenuMegaMenuAlignment();
    
    // Recalculate on window resize
    window.addEventListener('resize', subMenuMegaMenuAlignment);

    // Home slider (Swiper)
    // Note: This assumes Swiper is loaded as a global variable
    if (typeof Swiper !== 'undefined') {
        const homeSlider = new Swiper('.home-slider', {
            loop: true,
            loopedSlides: 2,
            speed: 750,
            spaceBetween: 200,
            pagination: {
                el: '.home-slider-pagination',
                clickable: true,
            },
            navigation: {
                nextEl: '.home-slider-next',
                prevEl: '.home-slider-prev',
            },
            autoplay: {},
        });
    }

    // Slick sliders
    if (typeof window.slick === 'function' || (typeof $ === 'function' && typeof $.fn.slick === 'function')) {
        // Helper function to initialize slick with vanilla JS
        const initSlick = (selector, options) => {
            if (typeof $ === 'function' && typeof $.fn.slick === 'function') {
                $(selector).slick(options);
            } else if (typeof window.slick === 'function') {
                document.querySelectorAll(selector).forEach(el => {
                    window.slick(el, options);
                });
            }
        };

        // Testimonial 
        initSlick('.testimonial-slider', {
            infinite: true,
            slidesToShow: 1,
            slidesToScroll: 1,
            prevArrow: '<button class="slick-prev"><i class="fa-solid fa-angle-left"></i></button>',
            nextArrow: '<button class="slick-next"><i class="fa-solid fa-angle-right"></i></button>'
        });

        initSlick('.testimonial-carousel', {
            infinite: true,
            slidesToShow: 3,
            slidesToScroll: 1,
            prevArrow: '<button class="slick-prev"><i class="fa-solid fa-angle-left"></i></button>',
            nextArrow: '<button class="slick-next"><i class="fa-solid fa-angle-right"></i></button>',
            responsive: [{
                breakpoint: 991,
                settings: {
                    slidesToShow: 2
                }
            },
            {
                breakpoint: 767,
                settings: {
                    slidesToShow: 1
                }
            }]
        });

        // Instagram feed carousel 1
        initSlick('.instafeed-carousel1', {
            infinite: true,
            slidesToShow: 5,
            slidesToScroll: 1,
            prevArrow: '<button class="slick-prev"><i class="fa-solid fa-angle-left"></i></button>',
            nextArrow: '<button class="slick-next"><i class="fa-solid fa-angle-right"></i></button>',
            responsive: [{
                breakpoint: 119,
                settings: {
                    slidesToShow: 4
                }
            }, {
                breakpoint: 991,
                settings: {
                    slidesToShow: 3
                }
            }, {
                breakpoint: 767,
                settings: {
                    slidesToShow: 2
                }
            }, {
                breakpoint: 479,
                settings: {
                    slidesToShow: 1
                }
            }]
        });

        // Instagram feed carousel 2
        initSlick('.instafeed-carousel2', {
            infinite: true,
            slidesToShow: 3,
            slidesToScroll: 1,
            prevArrow: '<button class="slick-prev"><i class="fa-solid fa-angle-left"></i></button>',
            nextArrow: '<button class="slick-next"><i class="fa-solid fa-angle-right"></i></button>',
            responsive: [{
                breakpoint: 767,
                settings: {
                    slidesToShow: 2
                }
            }, {
                breakpoint: 479,
                settings: {
                    slidesToShow: 1
                }
            }]
        });
    }

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




// Quick View Modal - Initialize slider when modal is shown
const quickViewModal = document.getElementById('quickViewModal');

if (quickViewModal) {
    // For Bootstrap 5
    quickViewModal.addEventListener('shown.bs.modal', function(e) {
        initializeQuickViewSlider();
    });
    
    // For Bootstrap 4 (fallback)
    if (typeof $ !== 'undefined' && typeof $.fn.modal !== 'undefined') {
        $('#quickViewModal').on('shown.bs.modal', function(e) {
            initializeQuickViewSlider();
        });
    }
}

function initializeQuickViewSlider() {
    const sliderElement = document.querySelector('.product-gallery-slider-quickview');
    
    if (sliderElement) {
        // Check if Slick is available and initialize
        if (typeof $ !== 'undefined' && typeof $.fn.slick !== 'undefined') {
            // Use jQuery Slick if available
            $('.product-gallery-slider-quickview').slick({
                dots: true,
                infinite: true,
                slidesToShow: 1,
                slidesToScroll: 1,
                prevArrow: '<button class="slick-prev"><i class="fa-solid fa-angle-left"></i></button>',
                nextArrow: '<button class="slick-next"><i class="fa-solid fa-angle-right"></i></button>'
            });
        } else if (typeof Slick !== 'undefined') {
            // Use vanilla Slick if available
            new Slick(sliderElement, {
                dots: true,
                infinite: true,
                slidesToShow: 1,
                slidesToScroll: 1,
                prevArrow: '<button class="slick-prev"><i class="fa-solid fa-angle-left"></i></button>',
                nextArrow: '<button class="slick-next"><i class="fa-solid fa-angle-right"></i></i></button>'
            });
        } else {
            // Fallback: Create a simple custom slider
            createCustomSlider(sliderElement);
        }
    }
}

// Custom slider implementation as fallback
function createCustomSlider(container) {
    const slides = container.querySelectorAll('.slide, .slick-slide, img');
    if (slides.length <= 1) return;
    
    let currentSlide = 0;
    
    // Create slider structure
    container.style.position = 'relative';
    container.style.overflow = 'hidden';
    
    // Hide all slides except first
    slides.forEach((slide, index) => {
        slide.style.display = index === 0 ? 'block' : 'none';
        slide.style.width = '100%';
    });
    
    // Create navigation buttons
    const prevButton = document.createElement('button');
    prevButton.className = 'slick-prev custom-prev';
    prevButton.innerHTML = '<i class="ti-angle-left"></i>';
    prevButton.style.cssText = `
        position: absolute;
        left: 10px;
        top: 50%;
        transform: translateY(-50%);
        z-index: 10;
        background: rgba(0,0,0,0.5);
        color: white;
        border: none;
        padding: 10px;
        cursor: pointer;
    `;
    
    const nextButton = document.createElement('button');
    nextButton.className = 'slick-next custom-next';
    nextButton.innerHTML = '<i class="fa-solid fa-angle-right"></i>';
    nextButton.style.cssText = `
        position: absolute;
        right: 10px;
        top: 50%;
        transform: translateY(-50%);
        z-index: 10;
        background: rgba(0,0,0,0.5);
        color: white;
        border: none;
        padding: 10px;
        cursor: pointer;
    `;
    
    // Create dots
    const dotsContainer = document.createElement('div');
    dotsContainer.className = 'slick-dots custom-dots';
    dotsContainer.style.cssText = `
        position: absolute;
        bottom: 20px;
        left: 50%;
        transform: translateX(-50%);
        display: flex;
        gap: 10px;
        z-index: 10;
    `;
    
    slides.forEach((_, index) => {
        const dot = document.createElement('button');
        dot.className = index === 0 ? 'active' : '';
        dot.style.cssText = `
            width: 12px;
            height: 12px;
            border-radius: 50%;
            border: none;
            background: ${index === 0 ? 'white' : 'rgba(255,255,255,0.5)'};
            cursor: pointer;
        `;
        dot.addEventListener('click', () => goToSlide(index));
        dotsContainer.appendChild(dot);
    });
    
    // Add elements to container
    container.appendChild(prevButton);
    container.appendChild(nextButton);
    container.appendChild(dotsContainer);
    
    // Navigation functions
    function goToSlide(index) {
        slides[currentSlide].style.display = 'none';
        dotsContainer.children[currentSlide].style.background = 'rgba(255,255,255,0.5)';
        dotsContainer.children[currentSlide].classList.remove('active');
        
        currentSlide = index;
        slides[currentSlide].style.display = 'block';
        dotsContainer.children[currentSlide].style.background = 'white';
        dotsContainer.children[currentSlide].classList.add('active');
    }
    
    function nextSlide() {
        const next = (currentSlide + 1) % slides.length;
        goToSlide(next);
    }
    
    function prevSlide() {
        const prev = (currentSlide - 1 + slides.length) % slides.length;
        goToSlide(prev);
    }
    
    // Event listeners
    prevButton.addEventListener('click', prevSlide);
    nextButton.addEventListener('click', nextSlide);
    
    // Auto-play (optional)
    // setInterval(nextSlide, 5000);
}

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

class NewsletterSubscription {
    constructor() {
        this.form = document.getElementById("newsletter-form")
        this.emailInput = document.getElementById("email")
        this.submitBtn = document.getElementById("submit-btn")
        this.messageDiv = document.getElementById("message")
        this.messageText = document.querySelector(".message-text")

        this.init()
    }

    init() {
        this.form.addEventListener("submit", (e) => this.handleSubmit(e))
    }

    async handleSubmit(e) {
        e.preventDefault()

        const email = this.emailInput.value.trim()

        if (!email) {
            this.showMessage("Please enter your email address", "error")
            return
        }

        if (!this.isValidEmail(email)) {
            this.showMessage("Please enter a valid email address", "error")
            return
        }

        this.setLoading(true)
        this.hideMessage()

        try {
            const result = await this.subscribeEmail(email)

            if (result.success) {
                this.showMessage(result.message, "success")
                this.emailInput.value = ""
            } else {
                this.showMessage(result.message, "error")
            }
        } catch (error) {
            this.showMessage("Something went wrong. Please try again.", "error")
        } finally {
            this.setLoading(false)
        }
    }

    async subscribeEmail(email) {
        // Simulate API call
        return new Promise((resolve) => {
            setTimeout(() => {
                // Simulate success/error randomly for demo
                const isSuccess = Math.random() > 0.3

                resolve({
                    success: isSuccess,
                    message: isSuccess
                        ? "Thank you for subscribing to our newsletter!"
                        : "Subscription failed. Please try again.",
                })
            }, 1000)
        })

        // Real API call example:
        /*
            try {
                const response = await fetch('/api/subscribe', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({ email })
                });
                
                const data = await response.json();
                return data;
            } catch (error) {
                throw new Error('Network error');
            }
            */
    }

    isValidEmail(email) {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
        return emailRegex.test(email)
    }

    showMessage(text, type) {
        this.messageText.textContent = text
        this.messageDiv.className = `message ${type}`
        this.messageDiv.classList.remove("hidden")
    }

    hideMessage() {
        this.messageDiv.classList.add("hidden")
    }

    setLoading(isLoading) {
        if (isLoading) {
            this.submitBtn.disabled = true
            this.submitBtn.classList.add("loading")
            this.submitBtn.textContent = "Subscribing..."
            this.emailInput.disabled = true
        } else {
            this.submitBtn.disabled = false
            this.submitBtn.classList.remove("loading")
            this.submitBtn.textContent = "Subscribe"
            this.emailInput.disabled = false
        }
    }
}

// Initialize when DOM is loaded
document.addEventListener("DOMContentLoaded", () => {
    new NewsletterSubscription()
})
