/*
Dynamic Tabs 1.0.3
Copyright (c) 2005 Rob Allen (rob at akrabat dot com)

Permission is hereby granted, free of charge, to any person
obtaining a copy of this software and associated documentation
files (the "Software"), to deal in the Software without restriction,
including without limitation the rights to use, copy, modify, merge,
publish, distribute, sublicense, and/or sell copies of the Software,
and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
DEALINGS IN THE SOFTWARE.

Change Log
22/Mar/2005: 1.0.0
	* Initial release.
28/Mar/2005: 1.0.1
	* Gracefully handle the case when there is no container id.
21/Dec/2005: 1.0.2
	* Patch from Imobach González Sosa to allow for multiple tab containers
17/Jan/2006: 1.0.3
	* Fixed version number at top of file
	* Added this Change Log
	* Fixed CSS as noted by Adam Fortuna of www.dyoit.com
	* Patch from Simon H to add tab ids for other scripts to hook into
*/

function getChildElementsByClassName(parentElement, className)
{
	var i, childElements, pattern, result;
	result = new Array();
	pattern = new RegExp("\\b"+className+"\\b");


	childElements = parentElement.getElementsByTagName('*');
	for(i = 0; i < childElements.length; i++)
	{
		if(childElements[i].className.search(pattern) != -1)
		{
			result[result.length] = childElements[i];
		}
	}
	return result;
}


function BuildTabs(containerId)
{
	var i, tabContainer, tabContents, tabHeading, title, tabElement;
	var divElement, ulElement, liElement, tabLink, linkText;


	// assume that if document.getElementById exists, then this will work...
	if(! eval('document.getElementById') ) return;

	tabContainer = document.getElementById(containerId);
	if(tabContainer == null)
		return;

	tabContents = getChildElementsByClassName(tabContainer, 'tab-content');
	if(tabContents.length == 0)
		return;

	divElement = document.createElement("div");
  	divElement.className = 'tab-header'
  	divElement.id = containerId + '-header';
	ulElement = document.createElement("ul");
  	ulElement.className = 'tab-list'

	tabContainer.insertBefore(divElement, tabContents[0]);
	divElement.appendChild(ulElement);

	for(i = 0; i < tabContents.length; i++)
	{
		tabHeading = getChildElementsByClassName(tabContents[i], 'tab');
		title = tabHeading[0].childNodes[0].nodeValue;

		// create the tabs as an unsigned list
		liElement = document.createElement("li");
		liElement.id = containerId + '-tab-' + i;

		tabLink = document.createElement("a");
		linkText = document.createTextNode(title);

		tabLink.className = "tab-item";

		tabLink.setAttribute("href","javascript://");
		tabLink.setAttribute( "title", tabHeading[0].getAttribute("title"));
		tabLink.onclick = new Function ("ActivateTab('" + containerId + "', " + i + ")");

		ulElement.appendChild(liElement);
		liElement.appendChild(tabLink);
		tabLink.appendChild(linkText);

		// remove the H1
		tabContents[i].removeChild
	}
}

function ActivateTab(containerId, activeTabIndex)
{
	var i, tabContainer, tabContents;

	tabContainer = document.getElementById(containerId);
	if(tabContainer == null)
		return;

	tabContents = getChildElementsByClassName(tabContainer, 'tab-content');
	if(tabContents.length > 0)
	{
		for(i = 0; i < tabContents.length; i++)
		{
			//tabContents[i].className = "tab-content";
			tabContents[i].style.display = "none";
		}

		tabContents[activeTabIndex].style.display = "block";


    		tabList = document.getElementById(containerId + '-list');
		tabs = getChildElementsByClassName(tabContainer, 'tab-item');
		if(tabs.length > 0)
		{
			for(i = 0; i < tabs.length; i++)
			{
				tabs[i].className = "tab-item";
			}

			tabs[activeTabIndex].className = "tab-item tab-active";
			tabs[activeTabIndex].blur();
		}
	}
}
BuildTabs('tab-container');
ActivateTab('tab-container', 0);
BuildTabs('tab-container-2');
ActivateTab('tab-container-2', 0);
