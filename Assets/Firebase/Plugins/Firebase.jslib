mergeInto(LibraryManager.library, {

  SendAds: function (type) {
    console.log('send ad from jslib')
    window.adBreak({
      type: UTF8ToString(type),
      name: 'openAds',
      beforeAd: () => {
        console.log('beforeAd');
      },
      afterAd: () => {
        console.log('afterAd');
      },
      adBreakDone: (placementInfo) => {
        console.log('adBreakDone');
        // gameInstance.emit(InterstitialAdEvent.AD_BREAK_DONE);
      },
    })
  },

  GetLinkApk: function () {
    const url = window.link_apk

    const bufferSize = lengthBytesUTF8(url) + 1;
    const buffer = _malloc(bufferSize);
    stringToUTF8(url, buffer, bufferSize);
    return buffer
  },

  LogEvent: function (type) {
    window.analytics.logEvent(UTF8ToString(type))
  }

});