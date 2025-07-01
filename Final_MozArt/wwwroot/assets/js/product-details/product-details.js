"use strict";

document.addEventListener('DOMContentLoaded', function() {

    
    document.querySelectorAll('[data-bg-image]').forEach(function(element) {
        const image = element.getAttribute('data-bg-image');
        element.style.backgroundImage = `url(${image})`;
    });
    
    document.querySelectorAll('[data-bg-color]').forEach(function(element) {
        const color = element.getAttribute('data-bg-color');
        element.style.backgroundColor = color;
    });

    
    
    const $ = window.jQuery; 
    if (typeof $ !== 'undefined' && $.fn.slick) {
        
        $('.product-carousel').slick({
            infinite: true,
            slidesToShow: 4,
            slidesToScroll: 1,
            focusOnSelect: true,
            prevArrow: '<button class="slick-prev"><i class="fa-solid fa-angle-left"></i></button>',
            nextArrow: '<button class="slick-next"><i class="fa-solid fa-angle-right"></i></button>',
            responsive: [{
                    breakpoint: 991,
                    settings: {
                        slidesToShow: 3
                    }
                },
                {
                    breakpoint: 767,
                    settings: {
                        slidesToShow: 2
                    }
                },
                {
                    breakpoint: 575,
                    settings: {
                        slidesToShow: 1
                    }
                }
            ]
        });

        $('.product-list-slider').slick({
            rows: 3,
            prevArrow: '<button class="slick-prev"><i class="fa-solid fa-angle-left"></i></button>',
            nextArrow: '<button class="slick-next"><<i class="fa-solid fa-angle-right"></i></button>'
        });

        $('.product-gallery-slider').slick({
            dots: true,
            infinite: true,
            slidesToShow: 1,
            slidesToScroll: 1,
            asNavFor: '.product-thumb-slider, .product-thumb-slider-vertical',
            prevArrow: '<button class="slick-prev"><i class="fa-solid fa-angle-left"></i></button>',
            nextArrow: '<button class="slick-next"><i class="fa-solid fa-angle-right"></i></button>'
        });
        
        $('.product-thumb-slider').slick({
            infinite: true,
            slidesToShow: 4,
            slidesToScroll: 1,
            focusOnSelect: true,
            asNavFor: '.product-gallery-slider',
            prevArrow: '<button class="slick-prev"><i class="fa-solid fa-angle-left"></i></button>',
            nextArrow: '<button class="slick-next"><i class="fa-solid fa-angle-right"></i></button>'
        });
        
        $('.product-thumb-slider-vertical').slick({
            infinite: true,
            slidesToShow: 3,
            slidesToScroll: 1,
            vertical: true,
            focusOnSelect: true,
            asNavFor: '.product-gallery-slider',
            prevArrow: '<button class="slick-prev"><i class="ti-angle-up"></i></button>',
            nextArrow: '<button class="slick-next"><i class="ti-angle-down"></i></button>'
        });
        
    } else {
        console.warn('jQuery veya Slick slider yüklü değil!');
    }

    
    function initQuantityButtons() {
        const qtyButtons = document.querySelectorAll('.qty-btn');
        
        qtyButtons.forEach(function(button) {
            button.removeEventListener('click', handleQuantityClick);
            button.addEventListener('click', handleQuantityClick);
        });
    }

    function handleQuantityClick(e) {
        e.preventDefault();
        
        const button = this;
        const parent = button.closest('.quantity-wrapper') || 
                      button.closest('.qty-wrapper') || 
                      button.closest('.product-quantity') ||
                      button.parentElement;
        
        let input = parent.querySelector('input[type="number"]') ||
                   parent.querySelector('input.qty') ||
                   parent.querySelector('input.quantity') ||
                   parent.querySelector('.qty-input') ||
                   parent.querySelector('input');

        if (!input) {
            input = button.previousElementSibling?.tagName === 'INPUT' ? button.previousElementSibling :
                   button.nextElementSibling?.tagName === 'INPUT' ? button.nextElementSibling : null;
        }

        if (!input) {
            console.warn('Quantity input bulunamadı:', button);
            return;
        }

        let currentValue = parseInt(input.value) || 1;
        let newValue = currentValue;

        if (button.classList.contains('plus') || 
            button.classList.contains('qty-plus') ||
            button.classList.contains('increase') ||
            button.getAttribute('data-action') === 'plus') {
            newValue = currentValue + 1;
        } else if (button.classList.contains('minus') || 
                  button.classList.contains('qty-minus') ||
                  button.classList.contains('decrease') ||
                  button.getAttribute('data-action') === 'minus') {
            newValue = currentValue > 1 ? currentValue - 1 : 1;
        }

        const minValue = parseInt(input.getAttribute('min')) || 1;
        const maxValue = parseInt(input.getAttribute('max')) || Infinity;

        if (newValue < minValue) newValue = minValue;
        if (newValue > maxValue) newValue = maxValue;

        input.value = newValue;

        const changeEvent = new Event('change', { bubbles: true });
        input.dispatchEvent(changeEvent);

        const customEvent = new CustomEvent('quantityChanged', {
            detail: { 
                oldValue: currentValue, 
                newValue: newValue,
                input: input,
                button: button
            },
            bubbles: true
        });
        input.dispatchEvent(customEvent);

        console.log(`Quantity changed from ${currentValue} to ${newValue}`);
    }

  
    initQuantityButtons();

   
    const observer = new MutationObserver(function(mutations) {
        let shouldReinit = false;
        
        mutations.forEach(function(mutation) {
            if (mutation.type === 'childList') {
                mutation.addedNodes.forEach(function(node) {
                    if (node.nodeType === 1) { // Element node
                        if (node.classList?.contains('qty-btn') || 
                            node.querySelector?.('.qty-btn')) {
                            shouldReinit = true;
                        }
                    }
                });
            }
        });

        if (shouldReinit) {
            setTimeout(initQuantityButtons, 100);
        }
    });

    observer.observe(document.body, {
        childList: true,
        subtree: true
    });

   
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

    
    const mobileMenuToggle = document.querySelector(".fa-bars");
    const mobileMenu = document.querySelector(".responsive-menu #menu");

    if (mobileMenuToggle && mobileMenu) {
        mobileMenuToggle.addEventListener("click", () => {
            mobileMenu.classList.toggle("active");
        });
    }

    
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

  
});


window.checkUserLogin = () => {
    return false;
};

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

window.logout = () => {
    localStorage.removeItem("userData");
    sessionStorage.removeItem("userToken");
    window.location.href = "home.html";
};

window.addEventListener('quantityChanged', function(e) {
    console.log('Quantity değişti:', e.detail);
    
});
