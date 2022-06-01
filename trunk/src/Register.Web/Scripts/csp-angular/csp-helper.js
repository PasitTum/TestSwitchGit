/**
 * Accepts either a URL or querystring and returns an object associating 
 * each querystring parameter to its value. 
 *
 * Returns an empty object if no querystring parameters found.
 */
function getUrlParams (urlOrQueryString) {
    if ((i = urlOrQueryString.indexOf('?')) >= 0) {
        const queryString = urlOrQueryString.substring(i+1);
        if (queryString) {
            return _mapUrlParams(queryString);
        } 
    }
    return {};
}

/**
 * Helper function for `getUrlParams()`
 * Builds the querystring parameter to value object map.
 *
 * @param queryString {string} - The full querystring, without the leading '?'.
 */
function _mapUrlParams (queryString) {
    return queryString    
      .split('&') 
      .map(function(keyValueString) { return keyValueString.split('=') })
      .reduce(function(urlParams, [key, value]) {
          if (Number.isInteger(parseInt(value)) && parseInt(value) == value) {
              urlParams[key.toLowerCase()] = parseInt(value);
          } else {
              urlParams[key.toLowerCase()] = decodeURI(value);
          }
          return urlParams;
      }, {});
}