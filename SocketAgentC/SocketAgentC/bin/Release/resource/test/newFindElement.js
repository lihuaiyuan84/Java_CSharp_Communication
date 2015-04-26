webdriver.WebDriver.prototype.findElement = function(locator) {
  var id;
  if ('nodeType' in locator && 'ownerDocument' in locator) {
    var element = /** @type {!Element} */ (locator);
    id = this.findDomElement_(element).then(function(element) {
      if (!element) {
        throw new bot.Error(bot.ErrorCode.NO_SUCH_ELEMENT,
            'Unable to locate element. Is WebDriver focused on its ' +
                'ownerDocument\'s frame?');
      }
      return element;
    });
  } else {
    locator = webdriver.Locator.checkLocator(locator);
    if (goog.isFunction(locator)) {
      id = this.findElementInternal_(locator, this);
    } else {
      var command = new webdriver.Command(webdriver.CommandName.FIND_ELEMENT).
          setParameter('using', locator.using).
          setParameter('value', locator.value);
      id = this.schedule(command, 'WebDriver.findElement(' + locator + ')');
    }
  }
    return new webdriver.WebElement(this, id);
};