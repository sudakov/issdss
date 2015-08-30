
// Ширина ScrollBar'uc
var scrollBarSize

window.onload = function () {
    if (location.href.search('Value.aspx') != -1 || location.href.search('Ranking.aspx') != -1) {
        // Установка корректного вида таблицы
        SetTableStyle()

        // Установка размера таблицы
        SetTableSize()

        // Обработчик скролинга
        document.getElementById('_DIV_Main').onscroll = _DIV_Main_OnScroll
    }
}

window.onresize = function () {
    if (location.href.search('Value.aspx') != -1 || location.href.search('Ranking.aspx') != -1) {
        SetTableSize()
    }
}

function _DIV_Main_OnScroll() {
    var scrollX = document.getElementById('_DIV_Main').scrollLeft
    var scrollY = document.getElementById('_DIV_Main').scrollTop

    with (document.getElementById('_DIV_SubHead').style) {
        position = 'relative'
        left = -scrollX + 'px'
    }

    with (document.getElementById('_DIV_SubLeft').style) {
        position = 'relative'
        top = -scrollY + 'px'
    }
}

function SetTableStyle() {
    // Объявление переменных
    var i, j, colHead, colMain, rowLeft, rowMain, divWidth = 0, w = [];
    var tableHead = document.getElementById('ctl00_MainContent__TBL_Head')
    var tableLeft = document.getElementById('_TBL_Left')
    var tableMain = document.getElementById('ctl00_MainContent__TBL_Main')

    // Стягивание div, чтобы ширина таблицы была минимальной,
    // таким образом определится минимальная ширина столбцов
    document.getElementById('_DIV_SubMain').style.width = 2 + 'px'
    document.getElementById('_DIV_SubHead').style.width = 2 + 'px'
    document.getElementById('_TD_Name').style.width = 2 + 'px'

    // Расчет ширины таблицы и запоминание ширин столбцов
    for (i = 0; i < tableMain.rows[0].cells.length; i++) {
        colHead = document.getElementById('ctl00_MainContent__COL_' + i)
        colMain = tableMain.rows[0].cells[i]
        if (colHead.offsetWidth < colMain.offsetWidth) {
            divWidth += colMain.offsetWidth
            w.push(colMain.offsetWidth)
        }
        else {
            divWidth += colHead.offsetWidth
            w.push(colHead.offsetWidth)
        }
    }

    // Растягивание div с запасом +1000, чтобы ширина таблицы не упиралась в div,
    // из-за чего могла бы съехать вёрстка
    document.getElementById('_DIV_SubMain').style.width = 1000 + divWidth + 'px'
    document.getElementById('_DIV_SubHead').style.width = 1000 + divWidth + 'px'

    // Сделать столбцы равной ширины
    for (i = 0; i < tableMain.rows[0].cells.length; i++) {
        document.getElementById('ctl00_MainContent__COL_' + i).style.width = w[i] + 'px'
        // Присутсвует дополнительный +1 к ширине строчной ячейки
        // в связи с наличием рамки border-right: 1px solid white; у шапки таблицы
        tableMain.rows[0].cells[i].style.width = w[i] + 1 + 'px'
    }

    // Возвращаем div'ам ширины получившихся таблиц
    // Без этого Opera и Chrome не понимают по хорошему :)
    document.getElementById('_DIV_SubMain').style.width = tableHead.offsetWidth + 'px'
    document.getElementById('_DIV_SubHead').style.width = tableHead.offsetWidth + 'px'
    
    // Сделать строки равной высоты
    for (i = 0; i < tableMain.rows.length; i++) {
        rowLeft = tableLeft.rows[i]
        rowMain = tableMain.rows[i]
        if (rowLeft.offsetHeight < rowMain.offsetHeight) {
            rowLeft.cells[0].style.height = rowMain.offsetHeight + 'px'
            rowMain.cells[0].style.height = rowMain.offsetHeight + 'px'
        }
        else {
            rowMain.cells[0].style.height = rowLeft.offsetHeight + 'px'
            rowLeft.cells[0].style.height = rowLeft.offsetHeight + 'px'
        }
    }
}

function SetTableSize() {
    // Объявление переменных
    var screenWidth, screenHeight, w, h

    // Размер экрана
    if (window.innerWidth) {
        screenWidth = window.innerWidth;
        screenHeight = window.innerHeight;
    } else if (document.documentElement && document.documentElement.clientWidth) {
        screenWidth = document.documentElement.clientWidth;
        screenHeight = document.documentElement.clientHeight;
    } else if (document.body && document.body.clientWidth) {
        screenWidth = document.body.clientWidth;
        screenHeight = document.body.clientHeight;
    }

    // Вычисление ширины ScrollBar'uc
    scrollBarSize = document.getElementById('_DIV_Main').offsetHeight - document.getElementById('_DIV_SubMain').offsetHeight

    // Присваивание значений
    h = screenHeight - 175 - document.getElementById('_DIV_Head').offsetHeight - document.getElementById('_TR_Head').offsetHeight - scrollBarSize
    w = screenWidth - 494 - scrollBarSize

    if (document.getElementById('_DIV_SubMain').offsetHeight > h) {
        document.getElementById('_DIV_Main').style.height = h + scrollBarSize + 'px'
        document.getElementById('_DIV_Left').style.height = h + scrollBarSize + 'px'
    }
    else {
        document.getElementById('_DIV_Main').style.height = document.getElementById('_DIV_SubMain').offsetHeight + scrollBarSize + 'px'
        document.getElementById('_DIV_Left').style.height = document.getElementById('_DIV_SubMain').offsetHeight + scrollBarSize + 'px'
    }

    if (document.getElementById('_DIV_SubMain').offsetWidth > w) {
        document.getElementById('_DIV_Main').style.width = w + scrollBarSize + 'px'
        document.getElementById('_DIV_Head').style.width = w + scrollBarSize + 'px'
    }
    else {
        document.getElementById('_DIV_Main').style.width = document.getElementById('_DIV_SubMain').offsetWidth + scrollBarSize + 'px'
        document.getElementById('_DIV_Head').style.width = document.getElementById('_DIV_SubMain').offsetWidth + scrollBarSize + 'px'
    }

    // Если таблица вылезает за пределы экрана, делаем _DIV_SubMain перетаскиваемым
    if (document.getElementById('_DIV_Main').offsetWidth - scrollBarSize < document.getElementById('_DIV_SubMain').offsetWidth ||
                document.getElementById('_DIV_Main').offsetHeight - scrollBarSize < document.getElementById('_DIV_SubMain').offsetHeight) {
        var dragObjects = document.getElementById('_DIV_SubMain')
        dragMaster1.makeDraggable(dragObjects)
        dragObjects.style.cursor = "pointer"
    }
    else
        document.getElementById('_DIV_SubMain').style.cursor = "default"
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
    var scrollOffset

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

        var x = e.pageX - mouseOffset.x - document.getElementById('_DIV_Main').offsetLeft
        var y = e.pageY - mouseOffset.y - document.getElementById('_DIV_Main').offsetTop
        var _DIV_MainWidth = document.getElementById('_DIV_Main').offsetWidth
        var _DIV_MainHeight = document.getElementById('_DIV_Main').offsetHeight
        var _DIV_SubMainWidth = document.getElementById('_DIV_SubMain').offsetWidth
        var _DIV_SubMainHeight = document.getElementById('_DIV_SubMain').offsetHeight

        if (_DIV_MainWidth - scrollBarSize < _DIV_SubMainWidth) {
            if (x > scrollOffset.x)
                x = scrollOffset.x
            if (x - scrollOffset.x < _DIV_MainWidth - _DIV_SubMainWidth - scrollBarSize)
                x = _DIV_MainWidth - _DIV_SubMainWidth - scrollBarSize + scrollOffset.x
        }
        else
            x = 0

        if (_DIV_MainHeight - scrollBarSize < _DIV_SubMainHeight) {
            if (y > scrollOffset.y)
                y = scrollOffset.y
            if (y - scrollOffset.y < _DIV_MainHeight - _DIV_SubMainHeight - scrollBarSize)
                y = _DIV_MainHeight - _DIV_SubMainHeight - scrollBarSize + scrollOffset.y
        }
        else
            y = 0

        document.getElementById('_DIV_Main').scrollLeft = scrollOffset.x - x
        document.getElementById('_DIV_Main').scrollTop = scrollOffset.y - y

        return false
    }

    function mouseDown(e) {
        e = fixEvent(e)
        if (e.which != 1) return

        dragObject = this
        mouseOffset = getMouseOffset(this, e)
        scrollOffset = { x: document.getElementById('_DIV_Main').scrollLeft, y: document.getElementById('_DIV_Main').scrollTop }

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