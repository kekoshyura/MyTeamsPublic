
const hand2note = {
    setPointerCapture: function (element, pointerId) {
        try {
            element.setPointerCapture(pointerId);
        } catch (error) {
            console.log(error);
        }
    },

    releasePointerCapture: function (element, pointerId) {
        element.releasePointerCapture(pointerId);
    },

    focusElement: function (element) {
        element.focus();
    },

    selectElement: function (element) {
        element.select();
    },

    stopPropagateMouseDown: function (element) {
        element.addEventListener('mousedown', function (e) {
            e.stopPropagation();
        });
    },

    stopPropagateMouseUp: function (element) {
        element.addEventListener('mouseup', function (e) {
            e.stopPropagation();
        });
    },

    stopPropagateClick: function (element) {
        element.addEventListener('click', function (e) {
            e.stopPropagation();
        });
    },

    getBoundingClientRectByXPath: function (xPath, contextElement) {
        let element = document.evaluate(xPath, contextElement, null, XPathResult.FIRST_ORDERED_NODE_TYPE).singleNodeValue;
        if (element === undefined)
            throw 'No element was found by the given xPath';
        return element.getBoundingClientRect();
    },

    getBoundingClientRectById: function (elementId) {
        return getElementById(elementId).getBoundingClientRect();
    },

    getBoundingClientRectByClass: function (className) {
        return getElementByClassName(className).getBoundingClientRect();
    },

    getBoundingClientRect: function (element) {
        if (!element)
            return { x: 0, y: 0, width: 0, height: 0 };
        return element.getBoundingClientRect();
    },

    getBoundingClientRectByClass: function (elementClass) {
        var element = getElementByClassName(elementClass);
        return element.getBoundingClientRect();
    },

    getParentBoundingClientRect: function (element) {
        if (!element || !element.parentNode)
            return { x: 0, y: 0, width: 0, height: 0 };

        return element.parentNode.getBoundingClientRect();
    },

    getChildWithClassBoundingClientRect: function (parentElement, childClass) {
        if (!parentElement)
            return { x: 0, y: 0, width: 0, height: 0 };
        var element = getFirstChildWithClass(parentElement, childClass);
        if (!element)
            return { x: 0, y: 0, width: 0, height: 0 };
        return element.getBoundingClientRect();
    },

    getWindowBoundingClientRect: function () {
        let element = document.getElementsByClassName("window")[0];
        if (element === undefined)
            throw 'No element with "window" class was found';
        return element.getBoundingClientRect();
    },

    /**
     * Finds the closest parent with a background color other than transparent.  
     * It detects the background color of the given element the user actually sees.
     */
    getVisualBackgroundColor: function (element) {
        do {
            if (element == null)
                return null;

            var background = window.getComputedStyle(element).backgroundColor;
            if (background != null && background !== "transparent" && background !== "rgba(0, 0, 0, 0)")
                return background;
            element = element.parentNode;
        }
        while (true);
    },

    setupCascadingMenuHandlers: function (menu, items) {
        menu.addEventListener('mouseleave', e => window.hand2note.sendMessage({ EventName: "MenuOnLeave" }), { once: true });
        items.addEventListener('mouseleave', function (e) {
            e.stopPropagation();
            window.hand2note.sendMessage({ EventName: "ItemsOnLeave" });
        },
            { once: true });
    },

    scrollToElementById: function (elementIndex, parent, itemsCount) {
        var elementHeight = parent.scrollHeight / itemsCount;
        var visibleElements = parent.clientHeight / elementHeight;
        if (elementIndex < visibleElements)
            return;
        parent.scrollTop = elementHeight * (elementIndex - visibleElements + 1);
        return;
        /* element.scrollIntoView doesn't work with virtualiztion
        element.scrollIntoView({
            behavior: 'auto',
            block: 'center',
            inline: 'center'
        });
        */
    },

    isElementVisibleById: function (elementId) {
        const element = document.getElementById(elementId);
        /*element can be absent in a DOM if virtualization is enabled*/
        if (!element)
            return false;
        const scrollableParent = getScrollableParent(element);
        const { top, bottom, height } = element.getBoundingClientRect();
        const parentRectangle = scrollableParent.getBoundingClientRect();

        return top <= parentRectangle.top
            ? parentRectangle.top - top <= height
            : bottom - parentRectangle.bottom <= height;

        function getScrollableParent(element) {
            let parent = element.parentNode;
            while (!parent.classList.contains("scrollable"))
                parent = parent.parentNode;
            if (!parent)
                console.log("Failed to find scrollable parent");
            return parent;
        }
    },

    getElementProperty: function (element, propertyName) {
        if (!element)
            return 0;
        return element[propertyName];
    },

    setElementProperty: function (element, propertyName, value) {
        if (!element)
            return;
        element[propertyName] = value;
    },

    setElementStyleProperty: function (element, propertyName, value) {
        if (!element)
            return;
        element.style.setProperty(propertyName, value);
    },

    getElementStyleProperty: function (element, propertyName) {
        return window.getComputedStyle(element)[propertyName];
    },

    appendToWindow: function (element) {
        document.getElementsByClassName("window")[0].appendChild(element);
    },

    appendToParent: function (child, parent) {
        parent.appendChild(child);
    },

    sendMessage: function (message) {
        //SendMessage doesn't work with WebView.WPF
        //window.external.sendMessage({ EventName: message });    
    },

    reloadLinks: function () {
        var links = document.getElementsByTagName("link");
        for (var cl in links) {
            var link = links[cl];
            link.href += "";
        }
    },

    loadLink: function (href, rel) {
        var link = document.createElement("link");
        link.href = href;
        link.rel = rel;
        document.head.appendChild(link);
    },

    getElementByClassName(className) {
        const result = document.getElementsByClassName(className)[0];
        if (result === undefined) throw className + ' element not found in the document';
        return result;
    },

    getElementById(id) {
        const result = document.getElementById(id);
        if (result === 'undefined')
            throw 'Element with id=' + id + ' not found';
        return result;
    },

    shiftElementById(elementId, x, y) {
        const element = getElementById(elementId);
        const resultX = parseFloat(element.style.left.replace('px', '')) + x;
        const resultY = parseFloat(element.style.top.replace('px', '')) + y;
        console.log(`move ${elementId} by x=${x}, y=${y}`);
        element.style.left = resultX + 'px'
        element.style.top = resultY + 'px';
    },

    adjustPlayerPopupWindowSize: function () {
        const statPageItems = document.getElementsByClassName("stat-page-item");
        if (statPageItems.length == 0)
            return { Width: 0, Height: 0 }
        let maxRight = 0;
        let maxBottom = 0;
        for (var i = 0; i < statPageItems.length; i++) {
            const item = statPageItems[i];
            const rect = item.getBoundingClientRect()
            if (rect.right > maxRight)
                maxRight = rect.right;
            if (rect.bottom > maxBottom)
                maxBottom = rect.bottom;
        }
        //12 and 8 is padding. To make the code clean, we could compute the padding of the root div.
        return { Width: maxRight + 12, Height: maxBottom + 8 };
    },

    setupGridHeaderThumb: function (thumb, header) {
        const minColumnWidth = 10;
        var startX = 0, xShift = 0;
        thumb.addEventListener('pointerdown', startResize);
        var gridParent = header.closest('.grid');
        var headerRow = header.closest("#header-row");
        var sizes = [];
        var headerId = 0;
        function startResize(e) {
            hand2note.setPointerCapture(thumb, e.pointerId);
            startX = e.clientX;
            startY = e.clientY;
            var headers = getChildrenWithClass(headerRow, 'grid__header-item-container');
            headerId = getElementIndex(headers, header);
            if (headerId < 0) {
                console.error("header was not found in grid headers collection");

                return;

            }
            sizes = headers.map(item => item.clientWidth);
            document.addEventListener('mousemove', onMouseMove);
            document.addEventListener('pointerup', onMouseUp);
        }
        function onMouseMove(e) {
            e.preventDefault();

            xShift = startX - e.clientX;
            startX = e.clientX;
            var newWidth = (header.clientWidth - xShift);
            if (newWidth >= minColumnWidth)
                gridParent.style.gridTemplateColumns = getGridColumns(sizes, headerId, newWidth);;
        }
        function onMouseUp(e) {
            hand2note.releasePointerCapture(thumb, e.pointerId);
            document.removeEventListener('mousemove', onMouseMove);
            document.removeEventListener('pointerup', onMouseUp);
        }

        function getChildrenWithClass(parent, cssClass) {
            var result = [];
            for (let i = 0; i < parent.childNodes.length; i++) {
                let element = parent.childNodes[i];
                if (element.nodeType != 1)
                    continue;
                if (element.classList.contains(cssClass)) {
                    result.push(element);
                }
            }
            return result;
        }
        function getElementIndex(htmlCollection, element) {
            for (let i = 0; i < htmlCollection.length; i++) {
                if (htmlCollection[i] == element)
                    return i;
            }
            return -1;
        }
        function getGridColumns(sizes, currentId, newSize) {
            var result = "";
            for (let i = 0; i < sizes.length - 1; i++) {
                if (i != currentId) {
                    result = result + `${sizes[i]}px `
                }
                else {
                    result = result + `${newSize}px `
                }
            }
            return result + `auto`;
        }
    },

    getFirstChildWithClass: function (parent, cssClass) {
        for (let i = 0; i < parent.children.length; i++) {
            let element = parent.children[i];
            if (element.classList.contains(cssClass)) {
                return element;
            }
        }
        return null;
    },

    resizeElement: function (element, delta) {
        element.style.width = `${element.clientWidth + delta}px`;
    },

    addElementClass: function (element, className) {
        element.classList.add(className);
    },

    removeElementClass: function (element, className) {
        element.classList.remove(className);

    },

    getWindowContentBoundingRect: function () {
        var windowContent = this.getElementByClassName("window__content");
        return windowContent.getBoundingClientRect();
    },

    onWindowContentSizeChanged: (handlerRef, handlerName) => {
        var windowContent = this.getElementByClassName("window__content");
        onSizeChanged(windowContent, handlerRef, handlerName);
    },

    onSizeChanged: (element, handlerRef, handlerName) => {
        const observer = new ResizeObserver(entries => {
            if (entries.length == 0) return;
            const entry = entries[0];
            if (entry.borderBoxSize === undefined || entry.borderBoxSize.length == 0)
                return;
            const newSize = entry.borderBoxSize[0];
            const newWidth = newSize.inlineSize;
            const newHeight = newSize.blockSize;
            //19/07/22 Yura. When element is removed from DOM (on tab switch etc), height and width is 0, so we just skip reporting it
            if (newWidth == 0 || newHeight == 0)
                return;
            const eventArgs = { newWidth: newWidth, newHeight: newHeight };
            entry.target.dispatchEvent(new CustomEvent('sizechanged', eventArgs))
            /*console.log(`Dispatched size changed. width=${newWidth}, height=${newHeight}`);*/
            handlerRef.invokeMethodAsync(handlerName, eventArgs);
        })
        observer.observe(element);
    },

    autoGrow: (element) => {
        element.style.height = "5px";
        element.style.height = (element.scrollHeight) + "px";
    },

    getMousePositionWithinDocument: function () {
        return { X: window.mousePositionX, Y: window.mousePositionY }
    },

    setupPopupWindow: function (handlerRef, clickHandler, mouseDownHandler, excludingClickSelectors, excludingMouseDownSelectors) {

        function isClickOnScrollbar(e) {
            let src = e.srcElement;
            let srcClientRect = getBoundingClientRect(src);
            return src.scrollHeight > src.clientHeight && srcClientRect.width <= e.clientX ||
                src.scrollWidth > src.clientWidth && srcClientRect.height <= e.clientY;
        }

        function onDocumentClick(e) {
            if (isClickOnScrollbar(e))
                return;

            if (excludingClickSelectors.some((selector) =>
                e.target.closest(`[${selector}]`)))
                return;
            if (excludingClickSelectors.some((selector) =>
                e.target.closest(`.${selector}`)))
                return;

            handlerRef.invokeMethodAsync(clickHandler);
        }

        function onDocumentMouseDown(e) {
            if (isClickOnScrollbar(e))
                return;

            if (excludingMouseDownSelectors.some((selector) =>
                e.target.closest(`[${selector}]`)))
                return;
            if (excludingMouseDownSelectors.some((selector) =>
                e.target.closest(`.${selector}`)))
                return;
            handlerRef.invokeMethodAsync(mouseDownHandler);
        }

        window.addEventListener('mousedown', onDocumentMouseDown);
        window.addEventListener('click', onDocumentClick);
    },
    setupKeyboardScope: function (element) {
        element.addEventListener('click', args => {
            if (args.target.closest('[keyboard-scope]') !== element ||
                args.target.closest('input') !== element.closest('input') ||
                args.target.closest('textarea') !== element.closest('textarea'))
                element.blur();
            else
                element.focus();
        });
    },
    isFocused: element => element === document.activeElement,
    setupWinningGraph: rootElement => {

        function getHighlightedPointElement() {
            return getFirstChildWithClass(rootElement, 'highlighted-point');
        }

        function setTransparencyOfHighlightedPoint(value) {
            let element = getHighlightedPointElement();
            if (!element) return;
            if (value)
                addElementClass(element, "highlighted-point_transparent");
            else
                removeElementClass(element, 'highlighted-point_transparent');
        }

        rootElement.addEventListener('mousemove', args => {
            var highlightedPointElement = getHighlightedPointElement();
            if (highlightedPointElement == null)
                return;

            area = highlightedPointElement.getBoundingClientRect();
            if (area.left <= args.clientX && args.clientX <= area.right)
                setTransparencyOfHighlightedPoint(true);
            else
                setTransparencyOfHighlightedPoint(false);
        });

        rootElement.addEventListener('mouseout', _ => setTransparencyOfHighlightedPoint(false));
    },

    setupThumb: function (element) {
        function startDragging(e) {
            parent.classList.add('isBeingResized');
            window.document.addEventListener('click', finishDragging);
        }

        function finishDragging(e) {
            window.document.removeEventListener('click', finishDragging);
            parent.classList.remove('isBeingResized');
        }

        var parent = element.parentElement;
        element.addEventListener('mousedown', startDragging);
    },

    setupMainDialog: function (element, handlerRef, mouseDownHandler, mouseUpHandler) {
        function onmousedown(e) {
            if (e.target.closest(`.dialog-container `))
                return;
            handlerRef.invokeMethodAsync(mouseDownHandler);
        }
        function onmouseup(e) {
            if (e.target.closest(`.dialog-container `))
                return;
            handlerRef.invokeMethodAsync(mouseUpHandler);
        }
        element.addEventListener('mousedown', onmousedown);
        element.addEventListener('mouseup', onmouseup);
    },

    setupTreeContainer: function (element, handlerRef, mouseDownHandler) {
        function onmousedown(e) {
            if (e.target.closest(`.tree-view__item`))
                return;
            handlerRef.invokeMethodAsync(mouseDownHandler);
        }
        element.addEventListener('mousedown', onmousedown);
    },

    getSelectionText: function () {
        return window.getSelection().toString();
    },

    insertTextInSelection: function (newText, element) {
        const [start, end] = [element.selectionStart, element.selectionEnd];
        element.setRangeText(newText, start, end, 'select');
        return element.value;
    },

    setupMouseLeaveHandler: function (element, elementRef, handler) {
        function onmouseleave(e) {
            elementRef.invokeMethodAsync(handler);
        }
        element.addEventListener('mouseleave', onmouseleave);
    }
}

getElementByClassName = hand2note.getElementByClassName;
getElementById = hand2note.getElementById;
onSizeChanged = hand2note.onSizeChanged;
onWindowContentSizeChanged = hand2note.onWindowContentSizeChanged;
getFirstChildWithClass = hand2note.getFirstChildWithClass;
addElementClass = hand2note.addElementClass;
removeElementClass = hand2note.removeElementClass;
fixMenuPosition = hand2note.fixMenuPosition;
getBoundingClientRect = hand2note.getBoundingClientRect;
getWindowBoundingClientRect = hand2note.getWindowBoundingClientRect;
appendToWindow = hand2note.appendToWindow;

document.addEventListener('mousemove', e => {
    window.mousePositionX = e.clientX;
    window.mousePositionY = e.clientY;
    document.mousePositionX = e.clientX;
    document.mousePositionY = e.clientY;
});

window.main = hand2note;