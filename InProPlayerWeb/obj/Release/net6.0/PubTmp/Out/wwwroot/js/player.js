﻿$(document).ready(function () {
    var modalerrorTitle = $('.modal-error-title'),
        modelerrorBody = $('.modal-error-body'),
        errorModal = $('#errorModal'),
        playerTrack = $("#player-track"),
        albumName = $("#albumName-marquee"),
        sArea = $("#s-area"),
        seekBar = $("#seek-bar"),
        trackTime = $("#track-time"),
        insTime = $("#ins-time"),
        sHover = $("#s-hover"),
        playPauseButton = $("#play-pause-button"),
        playPreviousTrackButton = $("#play-previous"),
        playNextTrackButton = $("#play-next"),
        i = playPauseButton.find("i"),
        tProgress = $("#current-time"),
        tTime = $("#track-length"),
        range = $("#range"),
        fileForm = $(".fileForm"),
        formFileMultiple = $("#formFileMultiple"),
        selectMusic = $(".selectMusic"),
        TextToSpeeckBtn = $("#TextToSpeeck"),
        speechText = $("[name='speechText']"),
        nowPlay = selectMusic.first().html(),
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
        nTime = 0,
        currentTime = 0,
        duration = 0, 
        flag = -1,
        tFlag = false,
        isPlay = false,
        intervalId = null;

    function playAjax() {
        $.ajax({
            type: "POST",
            url: "/Player/Play",
            data: {
                fileName: nowPlay,
                startTime: currentTime
            },
            dataType: "json",
            success: function (response) {
                duration = response;
            }
        });
        updateInterval();
    }

    function pauseAjax() {
        clearInterval(intervalId); // 停止 setInterval
        $.ajax({
            type: "POST",
            url: "/Player/Pause",
            data: { },
            dataType: "json",
            success: function (response) {
            }
        });
    }

    function playPause() {
        if (!isPlay) {
            isPlay = true;
            albumName.html(nowPlay);
            playerTrack.addClass("active");
            i.attr("class", "bi bi-pause-fill");
            updateCurrTime();
            playAjax();
        } else {
            isPlay = false;
            albumName.html("");
            playerTrack.removeClass("active");
            i.attr("class", "bi bi-play-fill");
            pauseAjax();
        }
    }

    function updateInterval() {
        intervalId = setInterval(function () {
            $.ajax({
                type: "POST",
                url: "/Player/GetInit",
                data: {},
                dataType: "json",
                success: function (response) {
                    currentTime = response["CurrentTime"];
                    updateCurrTime();
                    if (!response["isPlay"]) {
                        clearInterval(intervalId); // 停止 setInterval
                        updateCurrTime();
                    }
                }
            });
        }, 500);
    }

    function showHover(event) {
        seekBarPos = sArea.offset();
        seekT = event.clientX - seekBarPos.left;
        seekLoc = duration * (seekT / sArea.outerWidth());

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
        currentTime = seekLoc;
        updateCurrTime();
        pauseAjax();
        playAjax();
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

        curMinutes = Math.floor(currentTime / 60);
        curSeconds = Math.floor(currentTime - curMinutes * 60);

        durMinutes = Math.floor(duration / 60);
        durSeconds = Math.floor(duration - durMinutes * 60);

        playProgress = (currentTime / duration) * 100;

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

        if (playProgress >= 100) {
            clearInterval(intervalId); // 停止 setInterval
            currentTime = 0;
            isPlay = false;
            albumName.html("");
            playerTrack.removeClass("active");
            i.attr("class", "bi bi-play-fill");
            seekBar.width(0);
            tProgress.text("00:00");
        }
    }

    function selectTrack(i) {
        clearInterval(intervalId); // 停止 setInterval
        isPlay = false;
        selectMusic.removeAttr("style");
        
        $.ajax({
            type: "POST",
            url: "/Player/Track",
            data: {
                fileName: nowPlay,
                Track: i,
                flag: flag
            },
            dataType: "text",
            success: function (response) {
                flag += i;
                if (flag <= -1) flag = 0;
                nowPlay = response;
                $.each(selectMusic, function (k, v) {
                    if ($(v).html() === response) $(v).css("background", "lightgoldenrodyellow");
                }); 
                currentTime = 0;
                playPause();
            }
        });
    }

    function initPlayer() {
        fileForm.on("submit", checkFrom);
        playPauseButton.on("click", playPause);
        sArea.on("click", playFromClickedPos);
        sArea.mouseout(hideHover);

        playPreviousTrackButton.on("click", function () {
            selectTrack(-1);
        });
        playNextTrackButton.on("click", function () {
            selectTrack(1);
        });        
        sArea.mousemove(function (event) {
            showHover(event);
        });

        range.on("change", function () {
            $.ajax({
                type: "POST",
                url: "/Player/SetVolume",
                data: {
                    volume: $(this).val()
                },
                dataType: "json",
                success: function (response) {
                }
            });
        });

        selectMusic.on("click", function () {
            if (isPlay) {
                $.ajax({
                    type: "POST",
                    url: "/Player/Stop",
                    data: {},
                    async: false,
                    dataType: "json",
                    success: function (response) {
                    }
                });
            }

            selectMusic.removeAttr("style");
            $(this).css("background", "lightgoldenrodyellow");
            currentTime = 0;
            flag = 0;
            nowPlay = $(this).html();
            isPlay = true;
            albumName.html(nowPlay);
            playerTrack.addClass("active");
            i.attr("class", "bi bi-pause-fill");            
            playAjax();
        });

        $.ajax({
            type: "POST",
            url: "/Player/GetInit",
            data: { },
            dataType: "json",
            success: function (response) {
                if (response["isPlay"]) {
                    flag = 0;
                    nowPlay = response["FileName"];
                    duration = response["Duration"];
                    currentTime = response["CurrentTime"];
                    range.val(response["Volume"] * 100);

                    $.each(selectMusic, function (k, v) {
                        if ($(v).html() === nowPlay) $(v).css("background", "lightgoldenrodyellow");
                    }); 

                    isPlay = response["isPlay"];
                    albumName.html(nowPlay);
                    playerTrack.addClass("active");
                    i.attr("class", "bi bi-pause-fill");
                    updateInterval();
                }
            }
        });

        TextToSpeeckBtn.on("click", function () {
            $.ajax({
                type: "POST",
                url: "/Player/TextToSpeech",
                data: { speechText: speechText.val() },
                dataType: "json",
                success: function (response) {
                }
            });
        });
    }

    initPlayer();

    function checkFrom() {
        if (formFileMultiple.get(0).files[0]) {
            return true;
        } else {
            modalerrorTitle.html("上傳錯誤!(Upload Error!)");
            modelerrorBody.html("請選擇檔案");
            errorModal.modal('show');
            return false;
        }
    }
});
