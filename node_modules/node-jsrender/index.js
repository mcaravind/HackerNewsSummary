// Augment the jsRenderWrapper object with a new method
var JsRenderWrapper = (function () {
	var fs = require('fs'),
		jsviews = require('./lib/jsrender'),
		JsRenderWrapper = function () {
			// Create a custom property to store original template source in
			this._original = this._original || {};
		};

	/**
	 * Loads a template file asynchronously.
	 * @param {String} name The name to assign the template. 
	 * @param {String} path The path to the template file.
	 * @param {Function=} callback Optional. The callback method to execute on loading the template.
	 */
	JsRenderWrapper.prototype.loadFile = function (name, path, callback) {
		if (name) {
			if (path) {
				fs.readFile(path, {encoding: 'utf8'}, function (err, data) {
					if (!err) {
						var result = this.loadString(name, String(data));
						if (callback) { callback(false, result); }
					} else {
						if (callback) { callback(err); }
					}
				});
			} else {
				throw('Cannot loadFile in jsRender if no path is passed: loadFileSync(name, path, callback);');
			}
		} else {
			throw('Cannot loadFile in jsRender if no name is passed: loadFileSync(name, path, callback);');
		}
	};
	
	/**
	 * Loads a template file synchronously.
	 * @param {String} name The name to assign the template. 
	 * @param {String} path The path to the template file.
	 * @returns {Function} The compiled template. 
	 */
	JsRenderWrapper.prototype.loadFileSync = function (name, path) {
		if (name) {
			if (path) {
				return this.loadString(name, String(fs.readFileSync(path, 'utf8')));
			} else {
				throw('Cannot loadFileSync in jsRender if no path is passed: loadFileSync(name, path);');
			}
		} else {
			throw('Cannot loadFileSync in jsRender if no name is passed: loadFileSync(name, path);');
		}
	};
	
	/**
	 * Creates a template from a string.
	 * @param {String} name The name to assign the template. 
	 * @param {String} str The string that contains the template source code.
	 * @returns {Function} The compiled template. 
	 */
	JsRenderWrapper.prototype.loadString = function (name, str) {
		var self = this;
		
		if (name) {
			if (str) {
				// Store the original string
				self._original[name] = String(str);
				
				// Return the compiled template
				return jsviews.templates(name, str);
			} else {
				throw('Cannot loadString in jsRender if no template string (str) is passed: loadString(name, str);');
			}
		} else {
			throw('Cannot loadString in jsRender if no template name (name) is passed: loadString(name, str);');
		}
	};

	/**
	 * Gets the original template source that generated the compiled template identified
	 * by the name parameter passed.
	 * @param {String} name The name of the template who's original source should be returned.
	 * @returns {String}
	 */
	JsRenderWrapper.prototype.original = function (name) {
		return this._original[name];
	};
	
	// Define the accessor variables
	JsRenderWrapper.prototype.render = jsviews.render;
	JsRenderWrapper.prototype.helpers = jsviews.views.helpers;
	JsRenderWrapper.prototype.jsviews = jsviews;
	
	return JsRenderWrapper;
}());

module.exports = new JsRenderWrapper();