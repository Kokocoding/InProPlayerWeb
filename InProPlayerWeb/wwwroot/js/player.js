$(function () {
    var playerTrack = $("#player-track"),
        albumName = $("#album-name"),
        sArea = $("#s-area"),
        seekBar = $("#seek-bar"),
        trackTime = $("#track-time"),
        insTime = $("#ins-time"),
        sHover = $("#s-hover"),
        playPauseButton = $("#play-pause-button"),
        i = playPauseButton.find("i"),
        tProgress = $("#current-time"),
        tTime = $("#track-length"),
        range = $("#range"),
        seekT,
        seekLoc,
        seekBarPos,
        cM,
        ctMinutes,
        ctSeconds,
        curMinutes,
        curSeconds,
        durMinutes,
        durSeconds,
        playProgress,
        bTime,
        nTime = 0,
        buffInterval = null,
        tFlag = false,
        trackUrl = [
            "https://raw.githubusercontent.com/himalayasingh/music-player-1/master/music/2.mp3",
            "https://raw.githubusercontent.com/himalayasingh/music-player-1/master/music/1.mp3",
            "https://raw.githubusercontent.com/himalayasingh/music-player-1/master/music/3.mp3",
            "https://raw.githubusercontent.com/himalayasingh/music-player-1/master/music/4.mp3",
            "https://raw.githubusercontent.com/himalayasingh/music-player-1/master/music/5.mp3"
        ],
        nowPlay = $(".selectFile").first().html(),
        isPlay = false;

    range.on("change", function () {

    });

    $(".selectFile").on("click", function () {
        nowPlay = $(this).html();
        isPlay = true;
        albumName.html(nowPlay);
        playerTrack.addClass("active");
        checkBuffering();
        i.attr("class", "bi bi-pause-fill");
        $.ajax({
            type: "POST",
            url: "/Player/PlaySelector",
            data: {
                fileName: nowPlay
            },
            dataType: "json",
            success: function (response) {
                console.log(response);
            }
        });
    });

    function playPause() {
        if (!isPlay) {
            isPlay = true;
            albumName.html(nowPlay);
            playerTrack.addClass("active");
            checkBuffering();
            i.attr("class", "bi bi-pause-fill");
            $.ajax({
                type: "POST",
                url: "/Player/Play",
                data: {
                    fileName: nowPlay
                },
                dataType: "json",
                success: function (response) {                    
                }
            });
        } else {
            isPlay = false;
            albumName.html("");
            playerTrack.removeClass("active");
            clearInterval(buffInterval);
            i.attr("class", "bi bi-play-fill");
            $.ajax({
                type: "POST",
                url: "/Player/Stop",
                data: {
                    fileName: nowPlay
                },
                dataType: "json",
                success: function (response) {                    
                }
            });
        }
    }

    function showHover(event) {
        seekBarPos = sArea.offset();
        seekT = event.clientX - seekBarPos.left;
        seekLoc = audio.duration * (seekT / sArea.outerWidth());

        sHover.width(seekT);

        cM = seekLoc / 60;

        ctMinutes = Math.floor(cM);
        ctSeconds = Math.floor(seekLoc - ctMinutes * 60);

        if (ctMinutes < 0 || ctSeconds < 0) return;

        if (ctMinutes < 0 || ctSeconds < 0) return;

        if (ctMinutes < 10) ctMinutes = "0" + ctMinutes;
        if (ctSeconds < 10) ctSeconds = "0" + ctSeconds;

        if (isNaN(ctMinutes) || isNaN(ctSeconds)) insTime.text("--:--");
        else insTime.text(ctMinutes + ":" + ctSeconds);

        insTime.css({left: seekT, "margin-left": "-21px" }).fadeIn(0);
    }

    function hideHover() {
        sHover.width(0);
        insTime.text("00:00").css({left: "0px", "margin-left": "0px" }).fadeOut(0);
    }

    function playFromClickedPos() {
        audio.currentTime = seekLoc;
        seekBar.width(seekT);
        hideHover();
    }

    function updateCurrTime() {
        nTime = new Date();
        nTime = nTime.getTime();

        if (!tFlag) {
            tFlag = true;
            trackTime.addClass("active");
        }

        curMinutes = Math.floor(audio.currentTime / 60);
        curSeconds = Math.floor(audio.currentTime - curMinutes * 60);

        durMinutes = Math.floor(audio.duration / 60);
        durSeconds = Math.floor(audio.duration - durMinutes * 60);

        playProgress = (audio.currentTime / audio.duration) * 100;

        if (curMinutes < 10) curMinutes = "0" + curMinutes;
        if (curSeconds < 10) curSeconds = "0" + curSeconds;

        if (durMinutes < 10) durMinutes = "0" + durMinutes;
        if (durSeconds < 10) durSeconds = "0" + durSeconds;

        if (isNaN(curMinutes) || isNaN(curSeconds)) tProgress.text("00:00");
        else tProgress.text(curMinutes + ":" + curSeconds);

        if (isNaN(durMinutes) || isNaN(durSeconds)) tTime.text("00:00");
        else tTime.text(durMinutes + ":" + durSeconds);

        if (
            isNaN(curMinutes) ||
            isNaN(curSeconds) ||
            isNaN(durMinutes) ||
            isNaN(durSeconds)
        )
        trackTime.removeClass("active");
        else trackTime.addClass("active");

        seekBar.width(playProgress + "%");

        if (playProgress == 100) {
            i.attr("class", "bi bi-play-fill");
            seekBar.width(0);
            tProgress.text("00:00");
            clearInterval(buffInterval);
        }
    }

    function checkBuffering() {
        clearInterval(buffInterval);
        buffInterval = setInterval(function () {
            bTime = new Date();
            bTime = bTime.getTime();
        }, 100);
    }

    function initPlayer() {
        playPauseButton.on("click", playPause);
        sArea.mousemove(function (event) {
            showHover(event);
        });

        sArea.mouseout(hideHover);
        sArea.on("click", playFromClickedPos);
    }

    initPlayer();
});
