menu = {
    closeEvent: new Event('close'),

    close: function (element) {
        element.dispatchEvent(menu.closeEvent);
    },

    fixMenuPosition: function (menuRect, menuItems) {

        const gap = 8;
        var menuItemsRect = getBoundingClientRect(menuItems);

        var windowContent = this.getElementByClassName("window__content");
        var windowSize = windowContent.getBoundingClientRect();
        var newWidth = windowSize.width;
        var newHeight = windowSize.height;

        resizeWindow();

        function resizeWindow() {
            var menuRect = menuItemsRect;

            if (menuRect.width + 10 > windowSize.width) {
                newWidth = menuRect.width + 10;
            }
            if (menuRect.height + 10 > windowSize.height) {
                newHeight = menuRect.height + 10;
            }

            if (newWidth <= windowSize.width && newHeight <= windowSize.height)
                return;

            resizeWindowContent(newWidth, newHeight);
        }

        var windowClientArea = getWindowBoundingClientRect();

        var hiddenWidth = menuRect.left + menuItemsRect.width - newWidth + gap;
        var hiddenHeight = menuRect.bottom + menuItemsRect.height - newHeight + gap;
        var newLeft = 0;
        if (hiddenWidth > 0) {
            if (hiddenHeight > 0) {
                newLeft = menuRect.left - menuItemsRect.width;
            }
            else {
                newLeft = menuRect.left - hiddenWidth;
            }
        }
        else {
            if (hiddenHeight > 0) {
                if (menuRect.right + menuItemsRect.width - newWidth > 0)
                    newLeft = menuRect.left - menuItemsRect.width;
                else
                    newLeft = menuRect.right;
            }
            else {
                newLeft = menuRect.left;
            }
        }
        var newTop = 0;
        if (hiddenHeight > 0) {
            newTop = menuRect.bottom - hiddenHeight;
        }
        else {
            newTop = menuRect.bottom;
        }
        if (newLeft < gap)
            newLeft = gap;
        if (newTop < gap)
            newTop = gap;
        menuItems.style.setProperty("left", `${newLeft}px`);
        menuItems.style.setProperty("top", `${newTop}px`);
    },

    setupMenu: function (menu, menuHeader, menuItems, options, excludingSelectors) {

        function closeMenu() {
            removeElementClass(menuItems, 'menu_opened');
            addElementClass(menuItems, 'hidden');
            menuItems.removeEventListener('click', onMenuMouseOut);
            menuHeader.addEventListener('click', showMenu);
            menuHeader.removeEventListener('click', closeMenuOnClick);
            resizeWindowContent(0, 0);

            // workaround to make ctrl+z support available after menu clicked   
            var keboardScope = menu.closest('[keyboard-scope]');
            if (keboardScope != null)
                keboardScope.focus();
        }

        function closeMenuOnClick(e) {
            e.stopPropagation();
            closeMenu();
        }

        function showMenu(e) {
            if (e.target.closest('.context-menu'))
                return;
            //2022-07-08. Andrew. This caused the bug: menu doesn't close on click on another menu
            //In general, we should avoid giving basic components the responsibilities to stop propagation of events.
            //More higher components should be responsible for stopping the propagation.
            //e.showPropagation();
            addElementClass(menuItems, 'menu_opened');
            var menuRect = getBoundingClientRect(menu);
            fixMenuPosition(menuRect, menuItems);
            removeElementClass(menuItems, 'hidden');
            menuHeader.removeEventListener('click', showMenu);
            menuHeader.addEventListener('click', closeMenuOnClick);
        }

        function onDocumentClick(e) {

            if (excludingSelectors != null && excludingSelectors.some((selector) => e.target.closest(`.${selector}`)))
                return;

            if (e.target.closest('.menu-items__container span, .menu-items__container div')) {
                return;
            }

            if (e.target.closest('.menu') === menu || e.target.closest('.menu-header') === menuHeader)
                return;
            if (!menuItems.classList.contains('menu_opened')) {
                return;
            }
            if (options.hideOnClick && !e.target.closest("[interactive-menu-item]"))
                closeMenuOnClick(e);

            
            // Detect click inside
            if (menuItems.contains(e.target))
                return;
            // Detect click inside dropdown item
            if (e.target.closest('.menu-items__container'))
                return;

            //click was outside dialog, we should close menu
            closeMenuOnClick(e);
        }

        function onMenuMouseOut(e) {
            if (!window.document.elementFromPoint(e.clientX, e.clientY).closest('.menu-items__container'))
                closeMenu();
        }

        window.document.addEventListener('click', onDocumentClick);
        menuHeader.addEventListener('click', showMenu);
        addElementClass(menuItems, 'hidden');
        appendToWindow(menuItems);
        menu.addEventListener('close', closeMenu);

        if (options.hideOnMouseOut)
            menuItems.addEventListener('mouseout', onMenuMouseOut);
    },

    setupPopup: function (popup, options) {
        function closePopup() {
            removeElementClass(popup, 'popup_shown');
            window.document.removeEventListener('click', onDocumentClick);
        }

        function onDocumentClick(e) {
            if (popup.classList.contains('popup_shown') && !e.target.closest('.popup')) {
                if (options.excludingSelectors.some((selector) => e.target.closest(`.${selector}`)))
                    return;
                closePopup();
            }
        }

        function onPopupMouseOut(e) {
            var element = window.document.elementFromPoint(e.clientX, e.clientY);
            if (element.closest('.popup'))
                return;
            if (options.excludingSelectors.some((selector) => element.closest(`.${selector}`)))
                return;
            closePopup();
        }

        function onPopupClick(e) {
            if (!options.hideOnClick)
                return;
            if (options.excludingSelectors.some((selector) => e.target.closest(`.${selector}`)))
                return;
            closePopup();
        }

        window.document.addEventListener('click', onDocumentClick);
        if (options.hideOnMouseOut)
            popup.addEventListener('mouseout', onPopupMouseOut);
        if (options.hideOnClick)
            popup.addEventListener('click', onPopupClick);
    },

    showPopup: function (popup, button) {
        addElementClass(popup, 'popup_shown');
        var rect = getBoundingClientRect(button);
        fixMenuPosition(rect, popup);
    },

    /**
     * Opens context menu near the mouse position after clicking a DOM element with correspondent attributes.
     */
    setupContextMenuHandlers: function (contextMenuElement, attributes, excludeNestedElements) {
        if (attributes == null || Object.keys(attributes).length == 0)
            addListener(contextMenuElement.parentNode);
        else {
            let attributeSelector = '';
            for (let attribute in attributes)
                attributeSelector += '[' + attribute + '=' + "'" + attributes[attribute] + "'" + "]";
            for (const node of contextMenuElement.parentNode.querySelectorAll(attributeSelector))
                addListener(node);
        }

        function addListener(element) {
            var windowContent = this.getElementByClassName("window__content");
            hand2note.appendToParent(contextMenuElement, windowContent);

            element.setAttribute('context-menu-scope', '');
            element.addEventListener('mouseup', function (e) {
                //If there is inner context menu scope, then skip
                if (e.target != element && e.target.closest('[context-menu-scope]') != element)
                    return;

                if (e.button === 2 && (!excludeNestedElements || e.target === element)) {
                    //hide before position adjusted
                    contextMenuElement.style.visibility = "hidden";
                    contextMenuElement.style.display = "flex";
                    const menuRect = contextMenuElement.getBoundingClientRect();
                    const left = e.clientX - menuRect.x;
                    const top = e.clientY - menuRect.y;

                    resizeWindow();

                    contextMenuElement.style.visibility = "visible";

                    //Attaching event handler that closes the menu

                    window.document.addEventListener(
                        "click",
                        e => {
                            contextMenuElement.setAttribute("style", "display: none; left: 0; top: 0");
                            resizeWindowContent(0, 0);
                            var keboardScope = element.closest('[keyboard-scope]');
                            if (keboardScope != null)
                                keboardScope.focus();
                        },
                        { once: true });

                    window.document.addEventListener(
                        "mousedown",
                        e => {
                            if (e.button === 2)
                                contextMenuElement.setAttribute("style", "display: none; left: 0; top: 0");
                        },
                        { once: true });

                    function resizeWindow() {
                        var windowSize = windowContent.getBoundingClientRect();
                        var newWidth = windowSize.width;
                        var newHeight = windowSize.height;
                        var newMenuLeft = left;
                        var newMenuTop = top;
                        if (menuRect.width + 10 > windowSize.width) {
                            newWidth = menuRect.width + 10;
                            newMenuLeft = 5;
                        }
                        if (menuRect.height + 10 > windowSize.height) {
                            newHeight = menuRect.height + 10;
                            newMenuTop = 5;
                        }
                        var menuRight = newMenuLeft + menuRect.width;
                        var menuBottom = newMenuTop + menuRect.bottom;
                        if (menuRight > newWidth) {
                            newMenuLeft = newMenuLeft - (menuRight - newWidth) - 5;
                        }
                        if (menuBottom > newHeight) {
                            newMenuTop = newMenuTop - (menuBottom - newHeight) - 5;
                        }

                        contextMenuElement.style.left = `${newMenuLeft}px`;
                        contextMenuElement.style.top = `${newMenuTop}px`;

                        if (newWidth <= windowSize.width && newHeight <= windowSize.height)
                            return;

                        resizeWindowContent(newWidth, newHeight);
                    }
                }
            });
        }
    },

    setupToolTip: function (toolTipElement, content, delay, handler, onSetup) {
        var timeout;

        function initialSetup(e) {
            parentElement.removeEventListener('mouseenter', initialSetup);
            parentElement.addEventListener('mouseenter', showToolTip);
            handler.invokeMethodAsync(onSetup);
            showToolTip();
        }

        function showToolTip(e) {
            if (timeout != null) { clearTimeout(timeout); }

            timeout = setTimeout(function () {
                var parentRect = getBoundingClientRect(parentElement);
                //parent could be already removed from DOM after timeout
                if (parentRect.width == 0 || parentRect.height == 0)
                    return;
                addElementClass(content, 'tool-tip_shown')
                fixMenuPosition(parentRect, content);
                removeElementClass(content, 'hidden');
            }, delay);
        }

        function hideToolTip(e) {
            if (timeout != null) {
                clearTimeout(timeout);
                timeout = null;
            }
            removeElementClass(content, 'tool-tip_shown');
        }
        if (toolTipElement == null)
            return;
        var parentElement = toolTipElement.parentElement;
        if (parentElement == null)
            return;

        appendToWindow(content);
        addElementClass(content, 'hidden');

        parentElement.addEventListener('mouseleave', hideToolTip);
        parentElement.addEventListener('mousedown', hideToolTip);
        content.addEventListener('mousedown', hideToolTip);
        content.addEventListener('mouseleave', hideToolTip);
        parentElement.addEventListener('mouseenter', initialSetup);

    },

    hideToolTip: function (toolTipContent) {
        removeElementClass(toolTipContent, 'tool-tip_shown');
    },

    showToolTip: function (toolTipElement, content) {
        if (toolTipElement == null)
            return;
        var parentElement = toolTipElement.parentElement;
        if (parentElement == null)
            return;
        var parentRect = getBoundingClientRect(parentElement);
        //parent could be already removed from DOM after timeout
        if (parentRect.width == 0 || parentRect.height == 0)
            return;
        addElementClass(content, 'tool-tip_shown')
        fixMenuPosition(parentRect, content);
        removeElementClass(content, 'hidden');
    },

    setupCascadingMenuItem: function (menu, menuItems, parentContainer, options) {
        var timeout;
        function closeMenu() {
            removeElementClass(menuItems, 'menu_opened');
            addElementClass(menuItems, 'hidden');
        }

        function closeMenuWithDelay() {
            if (timeout != null) { clearTimeout(timeout); }
            timeout = setTimeout(function () {
                closeMenu();
            }, 200);
        }

        function showMenu(e) {
            if (timeout != null) {
                clearTimeout(timeout);
                timeout = null;
            }
            e.stopPropagation();
            addElementClass(menuItems, 'menu_opened');
            fixCascadingMenuItemPosition(menu, menuItems);
            removeElementClass(menuItems, 'hidden');
        }

        function itemsHovered(e) {
            if (timeout == null)
                return;
            clearTimeout(timeout);
            timeout = null;
        }

        function onDocumentClick(e) {
            if (menuItems.classList.contains('menu_opened') && options.hideOnClick)
                closeMenu();
        }

        function onMenuMouseOut(e) {
            if (!window.document.elementFromPoint(e.clientX, e.clientY).closest('.cascading-menu__submenu'))
                closeMenuWithDelay();
        }

        window.document.addEventListener('click', onDocumentClick);

        addElementClass(menuItems, 'hidden');
        appendToWindow(menuItems);
        menu.addEventListener('mouseover', showMenu);
        menuItems.addEventListener('mouseover', itemsHovered);
        if (options.hideOnMouseOut) {
            menu.addEventListener('mouseout', onMenuMouseOut);
            menuItems.addEventListener('mouseout', onMenuMouseOut);
        }

        function fixCascadingMenuItemPosition(menu, menuItems) {
            var menuRect = getBoundingClientRect(menu);
            var menuItemsRect = getBoundingClientRect(menuItems);

            var windowContent = this.getElementByClassName("window__content");
            var windowSize = windowContent.getBoundingClientRect();
            var newWidth = windowSize.width;
            var newHeight = windowSize.height;

            resizeWindow();

            function resizeWindow() {
                if (windowSize.width < menuItemsRect.width + menuRect.width + 10) {
                    var widthRequired = menuItemsRect.width + menuRect.width + (windowSize.width - menuRect.right) + 10;
                    if (widthRequired > windowSize.width) {
                        newWidth = widthRequired;
                    }
                }
                var heightRequired = menuItemsRect.height + 20;
                if (heightRequired > windowSize.height) {
                    newHeight = heightRequired;
                }

                if (newWidth <= windowSize.width && newHeight <= windowSize.height)
                    return;
                resizeWindowContent(newWidth, newHeight);

                var parentRect = getBoundingClientRect(parentContainer);
                var newMenuLeft = parentRect.left + newWidth - windowSize.width;
                parentContainer.style.setProperty("left", `${newMenuLeft}px`);
            }
            var menuRect = getBoundingClientRect(menu);
            var hiddenWidth = menuRect.right + menuItemsRect.width - newWidth;
            var hiddenHeight = menuRect.top + menuItemsRect.height - newHeight;
            var newLeft = 0;
            if (hiddenWidth > 0)
                newLeft = menuRect.left - menuItemsRect.width;
            else
                newLeft = menuRect.right;
            var newTop = 0;
            if (hiddenHeight > 0)
                newTop = menuRect.top - hiddenHeight;
            else
                newTop = menuRect.top;

            menuItems.style.setProperty("left", `${newLeft}px`);
            menuItems.style.setProperty("top", `${newTop}px`);
        }
    },

    resizeWindowContent: function (width, height) {
        var rowResizer = document.getElementsByClassName("row-resizer")[0];
        if (!rowResizer)
            return;
        var columnResizer = document.getElementsByClassName("column-resizer")[0];
        if (!columnResizer)
            return;
        rowResizer.style.setProperty("width", `${width}px`);
        columnResizer.style.setProperty("height", `${height}px`);
    }

}

addElementClass = hand2note.addElementClass;
removeElementClass = hand2note.removeElementClass;
fixMenuPosition = menu.fixMenuPosition;
getBoundingClientRect = hand2note.getBoundingClientRect;
getWindowBoundingClientRect = hand2note.getWindowBoundingClientRect;
appendToWindow = hand2note.appendToWindow;
window.main.menu = menu;
resizeWindowContent = hand2note.menu.resizeWindowContent;