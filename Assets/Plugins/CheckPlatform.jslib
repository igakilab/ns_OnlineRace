mergeInto(LibraryManager.library, {
    CheckPlatform: function () { 
    	if (window.navigator.platform == "MacIntel" && window.navigator.userAgent.indexOf("Safari") != -1 && window.navigator.userAgent.indexOf("Chrome") == -1) {
  			if (window.navigator.standalone !== undefined) {
    			// iPad OS Safari
    			unityInstance.SendMessage('GameManager', 'setSmartPhoneMode')
  			}
  		}
        var ua = window.navigator.userAgent.toLowerCase(); 
        if(ua.indexOf("android") !== -1 || ua.indexOf("ios") !== -1 || ua.indexOf("iphone") !== -1 || ua.indexOf("ipad") !== -1){
            unityInstance.SendMessage('GameManager', 'setSmartPhoneMode')
        }
    },
});