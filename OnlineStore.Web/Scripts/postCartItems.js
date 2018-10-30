document.onreadystatechange = function () {
    if (document.readyState === "interactive") {
        sendJson();
    }
}


function sendJson(parameters) {
    let cartData = JSON.parse(localStorage.getItem("cart"));
    for (let i = 0; i < cartData.length; i++) {
        cartData[i] = parseInt(cartData[i]);
    }
    let itemsId = getUniqIds(cartData);
    let cartItems = [];

    for (let item of itemsId) {
        let cartItem =
        {
            productId: item,
            count: countItems(item)
        };
        cartItems.push(cartItem);
    }
    document.getElementById("items").value = JSON.stringify(cartItems);
}

function getUniqIds(data) {
    let uniq = [];
    data.sort();
    let j = 0;
    uniq[0] = data[0];
    for (let i = 1; i < data.length; i++) {
        if (data[i] !== uniq[j]) {
            uniq[j + 1] = data[i];
            j += 1;
        }
    }
    return uniq;
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