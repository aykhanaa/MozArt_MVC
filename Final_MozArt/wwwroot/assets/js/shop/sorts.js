

//let skip = 0;
//const take = 3;
//let isSortedOrFiltered = false;

//const container = document.getElementById("shop-products");
//const loadMoreBtn = document.getElementById("loadMoreBtn");

//// UI Şablon funksiyası — BÜTÜN məhsullar üçün
//function renderProduct(item) {
//    return `
//        <div class="grid-item col featured">
//            <div class="product">
//                <div class="product-thumb">
//                    <a href="/Shop/Detail/${item.id}" class="image">
//                        <img src="/assets/img/product-details/${item.image}" alt="${item.name}" />
//                        <img class="image-hover" src="/assets/img/product-details/${item.hower || item.image}" alt="${item.name}" />
//                    </a>
//                </div>
//                <div class="product-info">
//                    <h6 class="title">
//                        <a href="/Shop/Detail/${item.id}">${item.name}</a>
//                    </h6>
//                    <span class="price">$${item.price}</span>
//                    <div class="product-buttons">
//                        <a href="/Shop/Detail/${item.id}" class="product-button hintT-top" data-hint="View">
//                            <i class="fas fa-search"></i>
//                        </a>
//                        <a href="#" class="product-button hintT-top" data-hint="Add to Cart">
//                            <i class="fas fa-shopping-cart"></i>
//                        </a>
//                    </div>
//                </div>
//            </div>
//        </div>`;
//}

//// İlk məhsul yüklənməsi
//document.addEventListener("DOMContentLoaded", () => {
//    loadProducts();
//});

//// Load More
//loadMoreBtn.addEventListener("click", function (e) {
//    e.preventDefault();
//    if (!isSortedOrFiltered) {
//        loadProducts();
//    }
//});

//// Load More üçün məhsul yükləyici
//async function loadProducts() {
//    try {
//        const response = await fetch(`/Product/LoadMore?skip=${skip}&take=${take}`);
//        const data = await response.json();

//        if (data.success) {
//            if (data.products.length === 0) {
//                loadMoreBtn.style.display = "none";
//                return;
//            }

//            data.products.forEach(p => {
//                const html = renderProduct({
//                    id: p.id,
//                    name: p.name,
//                    image: p.image,
//                    hower: p.hower || p.image,
//                    price: p.price
//                });
//                container.insertAdjacentHTML('beforeend', html);
//            });

//            skip += take;
//        } else {
//            alert("Error: " + data.message);
//        }
//    } catch (err) {
//        console.error("Error loading products:", err);
//    }
//}

//// Sort funksiyası
//document.getElementById('sortSelect').addEventListener('change', function () {
//    const sortKey = this.value;

//    // UI reset
//    skip = 0;
//    isSortedOrFiltered = true;
//    container.innerHTML = '';
//    loadMoreBtn.style.display = "none";

//    fetch(`/Shop/GetSortedProducts?sortKey=${sortKey}`)
//        .then(res => res.json())
//        .then(data => {
//            data.forEach(item => {
//                const html = renderProduct(item);
//                container.insertAdjacentHTML('beforeend', html);
//            });
//        })
//        .catch(err => console.error("Sort error:", err));
//});

//// Price filter
//const priceSlider = document.getElementById("priceSlider");
//const priceValue = document.getElementById("priceValue");

//priceSlider.addEventListener("input", () => {
//    const minPrice = priceSlider.value;
//    priceValue.textContent = minPrice;

//    // UI reset
//    skip = 0;
//    isSortedOrFiltered = true;
//    container.innerHTML = '';
//    loadMoreBtn.style.display = "none";

//    fetch(`/Shop/FilterByPrice?min=${minPrice}&max=10000`)
//        .then(res => res.json())
//        .then(data => {
//            if (data.length === 0) {
//                container.innerHTML = "<p>No products found in this range.</p>";
//                return;
//            }

//            data.forEach(item => {
//                const html = renderProduct(item);
//                container.insertAdjacentHTML("beforeend", html);
//            });
//        })
//        .catch(err => console.error("Price filter error:", err));
//});



let skip = 0;
const take = 3;
let isSortedOrFiltered = false;

const container = document.getElementById("shop-products");
const loadMoreBtn = document.getElementById("loadMoreBtn");
const priceSlider = document.getElementById("priceSlider");
const priceValue = document.getElementById("priceValue");
let $container = null;

// ✅ Isotope initialize
function initializeIsotope() {
    $container = $(container);

    $container.isotope({
        itemSelector: '.grid-item',
        layoutMode: 'fitRows',
        percentPosition: true,
        masonry: {
            columnWidth: '.grid-sizer'
        }
    });
}

// ✅ UI Şablonu
function renderProduct(item) {
    return `
        <div class="grid-item col featured">
            <div class="product">
                <div class="product-thumb">
                    <a href="/Shop/Detail/${item.id}" class="image">
                        <img src="/assets/img/product-details/${item.image}" alt="${item.name}" />
                        <img class="image-hover" src="/assets/img/product-details/${item.hower || item.image}" alt="${item.name}" />
                    </a>
                </div>
                <div class="product-info">
                    <h6 class="title">
                        <a href="/Shop/Detail/${item.id}">${item.name}</a>
                    </h6>
                    <span class="price">$${item.price}</span>
                    <div class="product-buttons">
                        <a href="/Shop/Detail/${item.id}" class="product-button hintT-top" data-hint="View">
                            <i class="fas fa-search"></i>
                        </a>
                        <a href="#" class="product-button hintT-top" data-hint="Add to Cart">
                            <i class="fas fa-shopping-cart"></i>
                        </a>
                    </div>
                </div>
            </div>
        </div>`;
}

// ✅ DOM Hazır olduqda
document.addEventListener("DOMContentLoaded", () => {
    initializeIsotope();
    loadProducts();
});

// ✅ Load More düyməsi
loadMoreBtn.addEventListener("click", function (e) {
    e.preventDefault();
    if (!isSortedOrFiltered) loadProducts();
});

// ✅ Məhsul Yükləmə
async function loadProducts() {
    try {
        const response = await fetch(`/Product/LoadMore?skip=${skip}&take=${take}`);
        const data = await response.json();

        if (data.success) {
            if (data.products.length === 0) {
                loadMoreBtn.style.display = "none";
                return;
            }

            const tempDiv = document.createElement("div");
            const newElements = [];

            data.products.forEach(p => {
                const html = renderProduct({
                    id: p.id,
                    name: p.name,
                    image: p.image,
                    hower: p.hower || p.image,
                    price: p.price
                });
                tempDiv.innerHTML = html;
                const element = tempDiv.firstElementChild;
                container.appendChild(element);
                newElements.push(element);
            });

            // ✅ Isotope append
            $container.isotope('appended', newElements);
            imagesLoaded(container, () => {
                $container.isotope('layout');
            });

            skip += take;
        }
    } catch (err) {
        console.error("Error loading products:", err);
    }
}

// ✅ Sort
document.getElementById('sortSelect').addEventListener('change', function () {
    const sortKey = this.value;

    skip = 0;
    isSortedOrFiltered = true;
    container.innerHTML = '<div class="grid-sizer col-1"></div>';
    loadMoreBtn.style.display = "none";

    fetch(`/Shop/GetSortedProducts?sortKey=${sortKey}`)
        .then(res => res.json())
        .then(data => {
            const newElements = [];
            const tempDiv = document.createElement("div");

            data.forEach(item => {
                const html = renderProduct(item);
                tempDiv.innerHTML = html;
                const element = tempDiv.firstElementChild;
                container.appendChild(element);
                newElements.push(element);
            });

            $container.isotope('reloadItems');
            $container.isotope('appended', newElements);
            imagesLoaded(container, () => {
                $container.isotope('layout');
            });
        })
        .catch(err => console.error("Sort error:", err));
});

// ✅ Price Filter
priceSlider.addEventListener("input", () => {
    const minPrice = priceSlider.value;
    priceValue.textContent = minPrice;

    skip = 0;
    isSortedOrFiltered = true;
    container.innerHTML = '<div class="grid-sizer col-1"></div>';
    loadMoreBtn.style.display = "none";

    fetch(`/Shop/FilterByPrice?min=${minPrice}&max=10000`)
        .then(res => res.json())
        .then(data => {
            if (data.length === 0) {
                container.innerHTML += "<p>No products found in this range.</p>";
                return;
            }

            const newElements = [];
            const tempDiv = document.createElement("div");

            data.forEach(item => {
                const html = renderProduct(item);
                tempDiv.innerHTML = html;
                const element = tempDiv.firstElementChild;
                container.appendChild(element);
                newElements.push(element);
            });

            $container.isotope('reloadItems');
            $container.isotope('appended', newElements);
            imagesLoaded(container, () => {
                $container.isotope('layout');
            });
        })
        .catch(err => console.error("Price filter error:", err));
});
