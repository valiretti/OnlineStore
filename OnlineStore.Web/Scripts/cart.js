$(document).ready(function () {
    $('.add_item').click(function (e) {
        e.preventDefault();
        var clickId = $(this).attr('data-id');
        addItem(clickId);
    });
});

function setCartData(o) {
    localStorage.setItem('cart', JSON.stringify(o));
}

function addItem(id) {
    let cartString = localStorage.getItem("cart");
    let cart = cartString ? JSON.parse(cartString) : [];
    cart.push(id);
    setCartData(cart);
}