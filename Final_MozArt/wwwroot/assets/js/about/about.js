// YouTube video ID çıxarmaq
function getYouTubeVideoId(url) {
    const regExp = /^.*(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|&v=)([^#&?]*).*/
    const match = url.match(regExp)
    return match && match[2].length === 11 ? match[2] : null
  }
  
  // Modal açmaq
  function openVideoModal(videoUrl) {
    const modal = document.getElementById("videoModal")
    const iframe = document.getElementById("videoIframe")
    const videoId = getYouTubeVideoId(videoUrl)
  
    if (videoId) {
      iframe.src = `https://www.youtube.com/embed/${videoId}?autoplay=1&rel=0&modestbranding=1`
      modal.classList.add("show")
      document.body.style.overflow = "hidden"
    }
  }
  
  // Modal bağlamaq
  function closeVideoModal() {
    const modal = document.getElementById("videoModal")
    const iframe = document.getElementById("videoIframe")
  
    modal.classList.remove("show")
    iframe.src = ""
    document.body.style.overflow = "auto"
  }
  
  // Səhifə yüklənəndə
  document.addEventListener("DOMContentLoaded", () => {
    // data-bg-image işləmək
    const videoBanner = document.querySelector(".video-banner")
    const bgImage = videoBanner.getAttribute("data-bg-image")
    if (bgImage) {
      videoBanner.style.backgroundImage = `url('${bgImage}')`
    }
  
    // Video popup link-ə klik
    const videoPopup = document.querySelector(".video-popup")
    if (videoPopup) {
      videoPopup.addEventListener("click", function (e) {
        e.preventDefault()
        const videoUrl = this.getAttribute("href")
        openVideoModal(videoUrl)
      })
    }
  
    // Modal bağlama düymələri
    const closeBtn = document.querySelector(".video-modal-close")
    if (closeBtn) {
      closeBtn.addEventListener("click", closeVideoModal)
    }
  
    // Modal arxa planına klik
    const modal = document.getElementById("videoModal")
    if (modal) {
      modal.addEventListener("click", function (e) {
        if (e.target === this) {
          closeVideoModal()
        }
      })
    }
  
    // ESC düyməsi
    document.addEventListener("keydown", (e) => {
      if (e.key === "Escape") {
        closeVideoModal()
      }
    })
  })



  class InstagramSlider {
    constructor(sliderId) {
      this.slider = document.getElementById(sliderId)
      this.prevBtn = document.getElementById("prevBtn")
      this.nextBtn = document.getElementById("nextBtn")
      this.currentIndex = 0
      this.itemsToShow = this.getItemsToShow()
      this.totalItems = this.slider.children.length
      this.isTransitioning = false
      this.eventsBound = false
  
      if (!this.slider) {
        console.error("Slider element not found!")
        return
      }
  
      this.init()
      this.bindEvents()
    }
  
    init() {
      this.cloneItems()
      this.updateSlider()
    }
  
    cloneItems() {
      // Clear any existing clones first
      const originalItems = Array.from(this.slider.children).slice(0, this.totalItems)
      this.slider.innerHTML = ""
  
      // Add original items back
      originalItems.forEach((item) => this.slider.appendChild(item))
  
      // Clone first few items and append to end
      for (let i = 0; i < this.itemsToShow; i++) {
        const clone = originalItems[i].cloneNode(true)
        this.slider.appendChild(clone)
      }
  
      // Clone last few items and prepend to beginning
      for (let i = this.totalItems - this.itemsToShow; i < this.totalItems; i++) {
        const clone = originalItems[i].cloneNode(true)
        this.slider.insertBefore(clone, this.slider.firstChild)
      }
  
      // Update current index to account for prepended items
      this.currentIndex = this.itemsToShow
      this.slider.style.transition = "transform 0.5s ease"
      this.slider.style.transform = `translateX(-${this.currentIndex * (100 / this.itemsToShow)}%)`
    }
  
    getItemsToShow() {
      const width = window.innerWidth
      if (width <= 479) return 1
      if (width <= 767) return 2
      if (width <= 991) return 3
      if (width <= 1199) return 4
      return 5
    }
  
    updateSlider() {
      const itemWidth = 100 / this.itemsToShow
      const translateX = -(this.currentIndex * itemWidth)
      this.slider.style.transform = `translateX(${translateX}%)`
    }
  
    next() {
      if (this.isTransitioning) return
  
      this.isTransitioning = true
      this.currentIndex++
      this.updateSlider()
  
      setTimeout(() => {
        if (this.currentIndex >= this.totalItems + this.itemsToShow) {
          this.slider.style.transition = "none"
          this.currentIndex = this.itemsToShow
          this.updateSlider()
  
          setTimeout(() => {
            this.slider.style.transition = "transform 0.5s ease"
            this.isTransitioning = false
          }, 50)
        } else {
          this.isTransitioning = false
        }
      }, 500)
    }
  
    prev() {
      if (this.isTransitioning) return
  
      this.isTransitioning = true
      this.currentIndex--
      this.updateSlider()
  
      setTimeout(() => {
        if (this.currentIndex < this.itemsToShow) {
          this.slider.style.transition = "none"
          this.currentIndex = this.totalItems
          this.updateSlider()
  
          setTimeout(() => {
            this.slider.style.transition = "transform 0.5s ease"
            this.isTransitioning = false
          }, 50)
        } else {
          this.isTransitioning = false
        }
      }, 500)
    }
  
    bindEvents() {
      if (this.eventsBound) return
      this.eventsBound = true
  
      // Remove any existing event listeners
      if (this.nextBtn) {
        this.nextBtn.replaceWith(this.nextBtn.cloneNode(true))
        this.nextBtn = document.getElementById("nextBtn")
      }
  
      if (this.prevBtn) {
        this.prevBtn.replaceWith(this.prevBtn.cloneNode(true))
        this.prevBtn = document.getElementById("prevBtn")
      }
  
      // Add single event listeners
      if (this.nextBtn) {
        this.nextBtn.addEventListener("click", (e) => {
          e.preventDefault()
          e.stopPropagation()
          this.next()
        })
      }
  
      if (this.prevBtn) {
        this.prevBtn.addEventListener("click", (e) => {
          e.preventDefault()
          e.stopPropagation()
          this.prev()
        })
      }
  
      // Handle window resize
      let resizeTimeout
      window.addEventListener("resize", () => {
        clearTimeout(resizeTimeout)
        resizeTimeout = setTimeout(() => {
          const newItemsToShow = this.getItemsToShow()
          if (newItemsToShow !== this.itemsToShow) {
            this.itemsToShow = newItemsToShow
            this.init()
          }
        }, 250)
      })
  
      // Touch/swipe support
      let startX = 0
      let startY = 0
      let endX = 0
      let endY = 0
  
      this.slider.addEventListener("touchstart", (e) => {
        startX = e.touches[0].clientX
        startY = e.touches[0].clientY
      })
  
      this.slider.addEventListener("touchend", (e) => {
        endX = e.changedTouches[0].clientX
        endY = e.changedTouches[0].clientY
        const diffX = startX - endX
        const diffY = startY - endY
  
        // Only trigger if horizontal swipe is greater than vertical
        if (Math.abs(diffX) > Math.abs(diffY) && Math.abs(diffX) > 50) {
          if (diffX > 0) {
            this.next()
          } else {
            this.prev()
          }
        }
      })
    }
  }
  
  // Global slider instance
  let sliderInstance = null
  
  // Initialize the slider when DOM is loaded
  document.addEventListener("DOMContentLoaded", () => {
    // Wait for all elements to be ready
    setTimeout(() => {
      sliderInstance = new InstagramSlider("instafeed-slider")
    }, 200)
  })


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
  