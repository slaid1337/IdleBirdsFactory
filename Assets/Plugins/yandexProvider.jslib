mergeInto(LibraryManager.library, {

  Auth: function() {
    auth();
  },

  GetData: function() {
    getUserData();
  },
  
  SetData : function(data){
    setUserData(UTF8ToString(data));
  },

  ShowCommonADV: function () {
    showFullscrenAd();
  },

  ShowRewardADV: function() {
    showRewardedAd();
  },

  ShowRewardADVUnlock: function() {
    showRewardedAdUnlock();
  },

  ShowRewardADVBooster: function() {
    showRewardedAdBooster();
  },
  
  GetLeaderBoardEntries: function(){
    GetLeaderBoardEntries();
  },
  
  SetLeaderBoard: function(score){
    SetLeaderBoard(score);
  }

});