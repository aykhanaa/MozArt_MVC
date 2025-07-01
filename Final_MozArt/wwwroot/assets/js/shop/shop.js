"use strict";

document.addEventListener('DOMContentLoaded', function () {
    /*--
        Commons Variables
    -----------------------------------*/
    const body = document.body;

    /*--
        Custom script to call Background
        Image & Color from html data attribute
    -----------------------------------*/
    document.querySelectorAll('[data-bg-image]').forEach(function (element) {
        const image = element.getAttribute('data-bg-image');
        element.style.backgroundImage = `url(${image})`;
    });

    document.querySelectorAll('[data-bg-color]').forEach(function (element) {
        const color = element.getAttribute('data-bg-color');
        element.style.backgroundColor = color;
    });

    /*--
        Shop Toolbar
    -----------------------------------*/

    // Filter Toggle
    document.querySelectorAll('.product-filter-toggle').forEach(function (toggle) {
        toggle.addEventListener('click', function (e) {
            e.preventDefault();

            const target = this.getAttribute('href');
            const targetElement = document.querySelector(target);

            // Toggle active class
            this.classList.toggle('active');

            // Slide toggle animation
            if (targetElement) {
                if (targetElement.style.display === 'none' || getComputedStyle(targetElement).display === 'none') {
                    // Slide down
                    targetElement.style.display = 'block';
                    targetElement.style.height = '0px';
                    targetElement.style.overflow = 'hidden';
                    targetElement.style.transition = 'height 0.3s ease';

                    // Force reflow
                    targetElement.offsetHeight;

                    // Animate to full height
                    targetElement.style.height = targetElement.scrollHeight + 'px';

                    // Clean up after animation
                    setTimeout(() => {
                        targetElement.style.height = '';
                        targetElement.style.overflow = '';
                        targetElement.style.transition = '';
                    }, 300);
                } else {
                    // Slide up
                    const height = targetElement.scrollHeight;
                    targetElement.style.height = height + 'px';
                    targetElement.style.overflow = 'hidden';
                    targetElement.style.transition = 'height 0.3s ease';

                    // Force reflow
                    targetElement.offsetHeight;

                    // Animate to 0 height
                    targetElement.style.height = '0px';

                    // Hide after animation
                    setTimeout(() => {
                        targetElement.style.display = 'none';
                        targetElement.style.height = '';
                        targetElement.style.overflow = '';
                        targetElement.style.transition = '';
                    }, 300);
                }
            }

            // Update perfect scrollbar
            updatePerfectScrollbar();
        });
    });



    /*--
        Custom Scrollbar (Perfect Scrollbar)
    -----------------------------------*/
    function initializePerfectScrollbar() {
        const customScrollElements = document.querySelectorAll('.customScroll');

        customScrollElements.forEach(function (element) {
            if (typeof PerfectScrollbar !== 'undefined') {
                // Vanilla Perfect Scrollbar
                element.perfectScrollbarInstance = new PerfectScrollbar(element, {
                    suppressScrollX: true
                });
            } else if (typeof $ !== 'undefined' && typeof $.fn.perfectScrollbar !== 'undefined') {
                // jQuery Perfect Scrollbar
                $(element).perfectScrollbar({
                    suppressScrollX: true
                });
            }
        });
    }

    function updatePerfectScrollbar() {
        const customScrollElements = document.querySelectorAll('.customScroll');
        customScrollElements.forEach(function (scrollElement) {
            if (scrollElement.perfectScrollbarInstance && typeof scrollElement.perfectScrollbarInstance.update === 'function') {
                scrollElement.perfectScrollbarInstance.update();
            } else if (typeof $ !== 'undefined' && typeof $.fn.perfectScrollbar !== 'undefined') {
                $(scrollElement).perfectScrollbar('update');
            }
        });
    }

    // Custom match height implementation
    function updateMatchHeight() {
        const matchHeightElements = document.querySelectorAll('[data-match-height]');
        const groups = {};

        // Group elements by data-match-height value
        matchHeightElements.forEach(element => {
            const group = element.getAttribute('data-match-height') || 'default';
            if (!groups[group]) groups[group] = [];
            groups[group].push(element);
        });

        // Set equal heights for each group
        Object.values(groups).forEach(group => {
            // Reset heights
            group.forEach(element => element.style.height = '');

            // Find max height
            let maxHeight = 0;
            group.forEach(element => {
                const height = element.offsetHeight;
                if (height > maxHeight) maxHeight = height;
            });

            // Apply max height to all elements in group
            group.forEach(element => {
                element.style.height = maxHeight + 'px';
            });
        });
    }

    /*--
        Isotope
    -----------------------------------*/
    let isotopeInstances = new Map();

    function initializeIsotope() {
        const isotopeGrids = document.querySelectorAll('.isotope-grid');

        isotopeGrids.forEach(function (grid) {
            // Wait for images to load
            const images = grid.querySelectorAll('img');
            let loadedImages = 0;
            const totalImages = images.length;

            function checkImagesLoaded() {
                loadedImages++;
                if (loadedImages === totalImages || totalImages === 0) {
                    initGrid();
                }
            }

            function initGrid() {
                if (typeof Isotope !== 'undefined') {
                    // Vanilla Isotope
                    const iso = new Isotope(grid, {
                        itemSelector: '.grid-item',
                        masonry: {
                            columnWidth: '.grid-sizer'
                        }
                    });
                    isotopeInstances.set(grid, iso);
                } else if (typeof $ !== 'undefined' && typeof $.fn.isotope !== 'undefined') {
                    // jQuery Isotope
                    $(grid).isotope({
                        itemSelector: '.grid-item',
                        masonry: {
                            columnWidth: '.grid-sizer'
                        }
                    });
                }
            }

            if (totalImages === 0) {
                initGrid();
            } else {
                images.forEach(img => {
                    if (img.complete) {
                        checkImagesLoaded();
                    } else {
                        img.addEventListener('load', checkImagesLoaded);
                        img.addEventListener('error', checkImagesLoaded);
                    }
                });
            }
        });
    }

    function updateIsotopeLayout() {
        const isotopeGrids = document.querySelectorAll('.isotope-grid');
        isotopeGrids.forEach(grid => {
            const instance = isotopeInstances.get(grid);
            if (instance && typeof instance.layout === 'function') {
                instance.layout();
            } else if (typeof $ !== 'undefined' && typeof $.fn.isotope !== 'undefined') {
                $(grid).isotope('layout');
            }
        });
    }

    // Isotope filter functionality
    document.querySelectorAll('.isotope-filter').forEach(function (filterContainer) {
        filterContainer.addEventListener('click', function (e) {
            if (e.target.tagName === 'BUTTON') {
                const button = e.target;
                const filterValue = button.getAttribute('data-filter');
                const targetSelector = filterContainer.getAttribute('data-target');
                const targetElement = document.querySelector(targetSelector);

                // Update active states
                button.classList.add('active');
                const siblings = filterContainer.querySelectorAll('button');
                siblings.forEach(sibling => {
                    if (sibling !== button) {
                        sibling.classList.remove('active');
                    }
                });

                // Apply filter
                if (targetElement) {
                    const instance = isotopeInstances.get(targetElement);
                    if (instance && typeof instance.arrange === 'function') {
                        instance.arrange({ filter: filterValue });
                    } else if (typeof $ !== 'undefined' && typeof $.fn.isotope !== 'undefined') {
                        $(targetElement).isotope({ filter: filterValue });
                    }
                }
            }
        });
    });

    /*--
        ion Range Slider
    -----------------------------------*/
    function initializeRangeSlider() {
        const rangeSliders = document.querySelectorAll('.range-slider');

        rangeSliders.forEach(function (slider) {
            if (typeof $ !== 'undefined' && typeof $.fn.ionRangeSlider !== 'undefined') {
                // jQuery ion Range Slider
                $(slider).ionRangeSlider({
                    skin: "learts",
                    hide_min_max: true,
                    type: 'double',
                    prefix: "$",
                });
            } else {
                // Custom range slider fallback
                createCustomRangeSlider(slider);
            }
        });
    }

    // Custom range slider implementation
    function createCustomRangeSlider(container) {
        const wrapper = document.createElement('div');
        wrapper.className = 'custom-range-slider';
        wrapper.style.cssText = `
            position: relative;
            height: 40px;
            background: #f0f0f0;
            border-radius: 20px;
            margin: 20px 0;
        `;

        const track = document.createElement('div');
        track.className = 'range-track';
        track.style.cssText = `
            position: absolute;
            top: 50%;
            left: 0;
            right: 0;
            height: 4px;
            background: #ddd;
            transform: translateY(-50%);
            border-radius: 2px;
        `;

        const fill = document.createElement('div');
        fill.className = 'range-fill';
        fill.style.cssText = `
            position: absolute;
            height: 100%;
            background: #007bff;
            border-radius: 2px;
            left: 0%;
            width: 100%;
        `;

        const minHandle = document.createElement('div');
        minHandle.className = 'range-handle range-handle-min';
        minHandle.style.cssText = `
            position: absolute;
            top: 50%;
            width: 20px;
            height: 20px;
            background: #007bff;
            border: 2px solid white;
            border-radius: 50%;
            cursor: pointer;
            transform: translate(-50%, -50%);
            left: 0%;
            z-index: 2;
        `;

        const maxHandle = document.createElement('div');
        maxHandle.className = 'range-handle range-handle-max';
        maxHandle.style.cssText = `
            position: absolute;
            top: 50%;
            width: 20px;
            height: 20px;
            background: #007bff;
            border: 2px solid white;
            border-radius: 50%;
            cursor: pointer;
            transform: translate(-50%, -50%);
            left: 100%;
            z-index: 2;
        `;

        const display = document.createElement('div');
        display.className = 'range-display';
        display.style.cssText = `
            text-align: center;
            margin-top: 10px;
            font-weight: bold;
        `;
        display.textContent = '$0 - $1000';

        let minValue = 0;
        let maxValue = 1000;
        let isDragging = false;
        let currentHandle = null;

        function updateDisplay() {
            display.textContent = `$${minValue} - $${maxValue}`;
            const minPercent = (minValue / 1000) * 100;
            const maxPercent = (maxValue / 1000) * 100;

            minHandle.style.left = minPercent + '%';
            maxHandle.style.left = maxPercent + '%';
            fill.style.left = minPercent + '%';
            fill.style.width = (maxPercent - minPercent) + '%';
        }

        function handleMouseDown(e, handle) {
            isDragging = true;
            currentHandle = handle;
            e.preventDefault();
        }

        function handleMouseMove(e) {
            if (!isDragging || !currentHandle) return;

            const rect = wrapper.getBoundingClientRect();
            const percent = Math.max(0, Math.min(100, ((e.clientX - rect.left) / rect.width) * 100));
            const value = Math.round((percent / 100) * 1000);

            if (currentHandle === minHandle) {
                minValue = Math.min(value, maxValue);
            } else {
                maxValue = Math.max(value, minValue);
            }

            updateDisplay();
        }

        function handleMouseUp() {
            isDragging = false;
            currentHandle = null;
        }

        minHandle.addEventListener('mousedown', (e) => handleMouseDown(e, minHandle));
        maxHandle.addEventListener('mousedown', (e) => handleMouseDown(e, maxHandle));
        document.addEventListener('mousemove', handleMouseMove);
        document.addEventListener('mouseup', handleMouseUp);

        track.appendChild(fill);
        wrapper.appendChild(track);
        wrapper.appendChild(minHandle);
        wrapper.appendChild(maxHandle);

        container.appendChild(wrapper);
        container.appendChild(display);

        updateDisplay();
    }

    // Initialize all components
    initializePerfectScrollbar();
    initializeIsotope();
    initializeRangeSlider();
});


// Tam işləyən və optimize edilmiş versiya
document.addEventListener('click', function (e) {
    const toggleButton = e.target.closest('.toggle[data-column]');

    if (toggleButton) {
        console.log('Toggle button clicked:', toggleButton);
        e.preventDefault();

        const column = toggleButton.getAttribute('data-column');
        const container = toggleButton.closest('.product-column-toggle');

        console.log('Column:', column);

        // Remove active from all toggles in container
        if (container) {
            container.querySelectorAll('.toggle').forEach(toggle => {
                toggle.classList.remove('active');
            });
        }

        // Add active to clicked toggle
        toggleButton.classList.add('active');

        // Update products grid
        const productsGrid = document.querySelector('.products');
        if (productsGrid) {
            // Remove all existing column classes
            const classList = Array.from(productsGrid.classList);
            classList.forEach(className => {
                if (className.startsWith('row-cols-xl-')) {
                    productsGrid.classList.remove(className);
                }
            });

            // Add new column class
            productsGrid.classList.add(`row-cols-xl-${column}`);

            console.log('Grid updated to:', column, 'columns');
            console.log('Grid classes:', productsGrid.className);

            // Trigger layout updates
            setTimeout(() => {
                // Dispatch resize event
                window.dispatchEvent(new Event('resize'));

                // Update isotope if properly initialized
                const isotopeGrids = document.querySelectorAll('.isotope-grid');
                isotopeGrids.forEach(grid => {
                    try {
                        if (typeof $ !== 'undefined' && typeof $.fn.isotope !== 'undefined') {
                            // Check if isotope is initialized on this element
                            const isotopeData = $(grid).data('isotope');
                            if (isotopeData) {
                                $(grid).isotope('layout');
                                console.log('Isotope layout updated');
                            } else {
                                console.log('Isotope not initialized on this grid, skipping...');
                            }
                        }
                    } catch (error) {
                        console.log('Isotope update failed:', error.message);
                    }
                });

                // Update match height if available
                if (typeof $ !== 'undefined' && typeof $.fn.matchHeight !== 'undefined') {
                    try {
                        $.fn.matchHeight._update();
                        console.log('Match height updated');
                    } catch (error) {
                        console.log('Match height update failed:', error.message);
                    }
                }

            }, 100);
        }
    }
});

// Isotope-u düzgün initialize etmək üçün əlavə kod
document.addEventListener('DOMContentLoaded', function () {
    // Isotope grid-ləri initialize et
    const isotopeGrids = document.querySelectorAll('.isotope-grid');

    isotopeGrids.forEach(grid => {
        if (typeof $ !== 'undefined' && typeof $.fn.isotope !== 'undefined') {
            // Images loaded olduqdan sonra isotope initialize et
            if (typeof $.fn.imagesLoaded !== 'undefined') {
                $(grid).imagesLoaded(function () {
                    $(grid).isotope({
                        itemSelector: '.grid-item',
                        masonry: {
                            columnWidth: '.grid-sizer'
                        }
                    });
                    console.log('Isotope initialized for grid:', grid);
                });
            } else {
                // Images loaded yoxdursa, kiçik delay ilə initialize et
                setTimeout(() => {
                    $(grid).isotope({
                        itemSelector: '.grid-item',
                        masonry: {
                            columnWidth: '.grid-sizer'
                        }
                    });
                    console.log('Isotope initialized for grid (delayed):', grid);
                }, 500);
            }
        }
    });

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
        window.addEventListener('scroll', function () {
            if (window.pageYOffset > 100) {
                scrollButton.style.display = 'block';
            } else {
                scrollButton.style.display = 'none';
            }
        });

        // Scroll to top when clicked
        scrollButton.addEventListener('click', function () {
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