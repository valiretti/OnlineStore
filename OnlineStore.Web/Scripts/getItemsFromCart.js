function getCart() {
    return localStorage.getItem('cart');
}

function getCartItems() {
    let itemsId = getCart();
    var request = new XMLHttpRequest;
    request.onreadystatechange = reqReadyStateChange;
    function reqReadyStateChange() {
        if (request.readyState === 4) {
            var status = request.status;
            if (status === 200) {
                displayItemsInCart(request.responseText);
            }
        }
    }
    request.open("GET", "/home/GetPhonesForCart?json=" + itemsId);
    request.send();
}


document.onreadystatechange = function () {
    if (document.readyState === "interactive") {
        getCartItems();
    }
}

function displayItemsInCart(data) {
    var cartContent = JSON.parse(data);
    if (cartContent.length !== 0) {
        var strResult = "<table><th>Название</th><th>Описание товара</th><th>Цена</th><th colspan='3'>Количество</th>";
        for (var item of cartContent) {
            strResult += "<tr><td>" +
                encodeHTML(item.Name) +
                "</td><td> " +
                encodeHTML(item.ProductDescription) +
                "</td><td>" +
                encodeHTML(item.Price) +
                "</td><td><a style='cursor:pointer;text-decoration: none;' data-id='" +
                item.Id +
                "'onclick='minusItem(this);'> - </a></td><td >" +
                countItems(item.Id) +
                "</td><td><a style='cursor:pointer;text-decoration: none;' data-id='" +
                item.Id +
                "'onclick='plusItem(this);'> + </a>" +
                "</td><td><a style=cursor:pointer data-item='" +
                item.Id +
                "'onclick='deleteItem(this);' >Удалить</a></td></tr> ";
        }
        strResult += "</table>";
        strResult += "<p><p/>";
        strResult += "<p class='col-md-offset-9 col-md-10' > Общая стоимость: &nbsp; <span style='font-size: 20px'><strong>" +
            totalCost(cartContent) + " руб.</strong></span></p>";

        document.getElementById('cart_content').innerHTML = strResult;
        var table = document.querySelector("#cart_content table");
        table.className = "table";
        document.getElementById('order').setAttribute("style", "display:block;");

    } else {
        var str = "<p>Корзина пуста</p>";
        document.getElementById('cart_content').innerHTML = str;
        document.getElementById('order').setAttribute("style", "display:none;");

    }
}

var encodeHTML = (function () {
    var encodeHTMLmap = {
        "&": "&amp;",
        "'": "&#39;",
        '"': "&quot;",
        "<": "&lt;",
        ">": "&gt;"
    };
    function encodeHTMLmapper(ch) {
        return encodeHTMLmap[ch];
    }
    return function (text) {
        return text.replace(/[&"'<>]/g, encodeHTMLmapper);
    };

})();

function orderProduct() {
    document.location.href = "/home/Order";
}

function totalCost(cartContent) {
    let cost = 0.0;
    for (let item of cartContent) {
        cost += parseFloat(item.Price)*countItems(item.Id);
    }
    return cost;
}

function countItems(id) {
    let count = 0;
    let cartString = localStorage.getItem("cart");
    let cart = cartString ? JSON.parse(cartString) : [];
    for (var item of cart) {
        if (item == id) {
            count += 1;
        }
    }
    return count;
}

function deleteItem(el) {
    var id = $(el).attr('data-item');
    let cartString = localStorage.getItem("cart");
    let cart = cartString ? JSON.parse(cartString) : [];
    for (var i = cart.length - 1; i >= 0; i--) {
        if (cart[i] === id) {
            cart.splice(i, 1);
        }
    }
    setCartData(cart);
    getCartItems();
}

function plusItem(elem) {
    var id = $(elem).attr('data-id');
    addItem(id);
    getCartItems();
}

function minusItem(elem) {
    var id = $(elem).attr('data-id');
    let cartString = localStorage.getItem("cart");
    let cart = cartString ? JSON.parse(cartString) : [];
    for (var i = cart.length - 1; i >= 0; i--) {
        if (cart[i] === id) {
            cart.splice(i, 1);
            break;
        }
    }
    setCartData(cart);
    getCartItems();

}


