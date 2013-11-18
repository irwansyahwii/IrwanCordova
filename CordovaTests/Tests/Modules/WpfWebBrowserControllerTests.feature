Feature: WebBrowserControllerTests

@mytag
Scenario: Make sure IHtmlInteropClass's CordovaCallback property is properly set
	Given an instance of IHtmlInteropClass	
	When WpfWebBrowserController is instantiated
	Then IHtmlInteropClass's CordovaCallback propery must be set with the WpfWebBrowserController instance

