function updateQuantity(id, action) {
    let url = action === "plus" ? `/basket/plusicon?id=${id}` : `/basket/minusicon?id=${id}`;

    $.ajax({
        url: url,
        type: "POST",
        success: function (res) {
            if (res.countItem <= 0) {
                $(`button[onclick="updateQuantity(${id}, '${action}')"]`).closest("tr").remove();
            } else {
                const row = $(`button[onclick="updateQuantity(${id}, '${action}')"]`).closest("tr");
                row.find(".input-qty").val(res.countItem);
                row.find(".subtotal span").text(`$${res.productTotalPrice.toFixed(2)}`);
            }

            $(".basket-count").text(res.countBasket);
            $(".amount").text(`$${res.grandTotal.toFixed(2)}`);

            if (res.countBasket === 0) {
                $(".cart-form").remove();
                $(".cart-totals").remove();
                $(".container").append('<h4>Your basket is empty.</h4><a href="/Shop/Index" class="btn btn-dark mt-3">Go to Shop</a>');
            }
        }
    });
}

function removeItem(id) {
    $.ajax({
        url: `/basket/delete?id=${id}`,
        type: "POST",
        success: function (res) {
            $(`button[onclick="removeItem(${id})"]`).closest("tr").remove();
            $(".basket-count").text(res.count);
            $(".amount").text(`$${res.grandTotal.toFixed(2)}`);

            if (res.count === 0) {
                $(".cart-form").remove();
                $(".cart-totals").remove();
                $("#empty-message").removeClass("d-none");
            }
        }
    });
}

