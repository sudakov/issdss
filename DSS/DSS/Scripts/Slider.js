var sliders = []
var valuesTB = []

//function textChanged(e) {
//    var id = document.getElementById('_INPUT_ID').value
//    var s = valuesTB[id].value
//    if (e.which > 47 && e.which < 58) {
//        if (s < 0)
//            s = 0
//        if (s > 100)
//            s = 100
//        SetValue(id, s)
//    }
//    else
//        switch (e.which) { 
//            case 8: 
//        }  valuesTB[id].value = s.slice(0, s.length - 1)
//    //alert(e.which)
//}

window.onload = function () {
    var dragObjects = document.getElementsByTagName('img')
    for (var i = 0; i < dragObjects.length; i++) {
        if (dragObjects[i].id.search('_IMG_Pointer') != -1) {
            dragMaster1.makeDraggable(dragObjects[i])
            sliders.push(dragObjects[i])
        }
    }

    var inputs = document.getElementsByTagName('input')
    for (var i = 0; i < inputs.length; i++) {
        if (inputs[i].id.search('_TB_Value') != -1)
            valuesTB.push(inputs[i])
    }
    
    var divPointerWidth = document.getElementById('_DIV_Pointer').offsetWidth - 10
    for (var i = 0; i < sliders.length; i++) {
//        valuesTB[i].onkeydown = textChanged
        if (valuesTB[i].value < 0)
            valuesTB[i].value = 0
        if (valuesTB[i].value > 100)
            valuesTB[i].value = 100
        with (sliders[i].style) {
            position = 'relative'
            left = Math.floor(valuesTB[i].value * divPointerWidth / 100) + 'px'
        }
    }
}

window.onresize = function () {
    for (var i = 0; i < sliders.length; i++)
        SetValue(i, valuesTB[i].value)
}

function fixEvent(e) {
    // получить объект событие для IE
    e = e || window.event

    // добавить pageX/pageY для IE
    if (e.pageX == null && e.clientX != null) {
        var html = document.documentElement
        var body = document.body
        e.pageX = e.clientX + (html && html.scrollLeft || body && body.scrollLeft || 0) - (html.clientLeft || 0)
        e.pageY = e.clientY + (html && html.scrollTop || body && body.scrollTop || 0) - (html.clientTop || 0)
    }

    // добавить which для IE
    if (!e.which && e.button) {
        e.which = e.button & 1 ? 1 : (e.button & 2 ? 3 : (e.button & 4 ? 2 : 0))
    }

    return e
}

var dragMaster1 = (function () {

    var dragObject
    var mouseOffset

    function getMouseOffset(target, e) {
        var docPos = getPosition(target)
        return { x: e.pageX - docPos.x, y: e.pageY - docPos.y }
    }

    function mouseUp() {
        dragObject = null

        // clear events
        document.onmousemove = null
        document.onmouseup = null
        document.ondragstart = null
        document.body.onselectstart = null
    }

    function mouseMove(e) {
        e = fixEvent(e)

        var x = e.pageX - mouseOffset.x - document.getElementById('_DIV_Pointer').offsetLeft
        var divPointerWidth = document.getElementById('_DIV_Pointer').offsetWidth - 10
        if (x < 0)
            x = 0
        if (x > divPointerWidth)
            x = divPointerWidth

        var s = document.getElementById('_INPUT_ID').value
        valuesTB[s].value = Math.floor(100 * x / divPointerWidth)

        with (dragObject.style) {
            position = 'relative'
            left = x + 'px'
        }
        return false
    }

    function mouseDown(e) {
        e = fixEvent(e)
        if (e.which != 1) return

        dragObject = this
        mouseOffset = getMouseOffset(this, e)

        document.onmousemove = mouseMove
        document.onmouseup = mouseUp

        // отменить перенос и выделение текста при клике на тексте
        document.ondragstart = function () { return false }
        document.body.onselectstart = function () { return false }

        return false
    }

    return {
        makeDraggable: function (element) {
            element.onmousedown = mouseDown
        }
    }

} ())

function getPosition(e) {
    var left = 0
    var top = 0

    while (e.offsetParent) {
        left += e.offsetLeft
        top += e.offsetTop
        e = e.offsetParent
    }

    left += e.offsetLeft
    top += e.offsetTop

    return { x: left, y: top }
}

function SetValue(id, value) {
    valuesTB[id].value = value
    var divPointerWidth = document.getElementById('_DIV_Pointer').offsetWidth - 10
    with (sliders[id].style) {
        position = 'relative'
        left = Math.floor(value * divPointerWidth / 100) + 'px'
    }
}