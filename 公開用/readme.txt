視聴プラグイン
-------------------------------
NCV用のニコ生視聴プラグインです。


使い方
------
NCVのpluginsフォルダ内に「ncvPlayPlugin.dll」と「視聴プラグイン」フォルダを配置します。
最初にツール→オプションメニューからログイン設定と、
VLCやMPCなどのプレイヤーの設定が必要です。
放送URL欄にURLを入力して「視聴」ボタンを押すと視聴を開始します。
タイムシフトの視聴はできません。

注意
------
このプラグインでのHLS方式での視聴中は同じアカウントを使ってブラウザでの視聴及び録画はできません。

引数
------
視聴プラグイン.exe [lv番号] [設定]

「-設定項目=設定値」の形式で指定します。
「ニコ生新配信録画ツール（仮.config」内の設定項目とタイムシフト録画設定を起動時に指定できます。

例： -qualityRank=0,1,2,3,4,5 -ts-start=5m"
<br>
主な設定項目

browserNum
ログイン方法 1=アカウントログイン 2=ブラウザクッキー

accountId
メールアドレス アカウントログイン時に使うメールアドレス

accountPass
パスワード アカウントログイン時に使うパスワード

qualityRank
画質の優先順位 0=3Mbps(super_high)
	 1=2Mbps(high・高画質) 2=1Mbps(normal・低画質) 3=384kbps(low) 
	 4=192kbps(super_low) 5=音声のみ(audio_high) 
	 として「,」区切りで指定。なるべく高画質の場合は「0,1,2,3,4,5」
EngineMode
録画エンジン 0-標準のHLS録画 2-RTMP録画

latency
遅延 0.5 1.0 1.5 3.0

anotherPlayerPath
外部プレイヤーのパス パス文字列

Isminimized
最小化状態で起動　true/false

IsMinimizeNotify
最小化時はタスクトレイに収納　true/false


タイムシフト
いずれかを指定した場合は録画時にタイムシフト録画設定・追っかけ録画設定ウィンドウが開かなくなります
ts-start
録画開始時間 [時間]h[分]m[秒]s形式で指定　例 「12h52m3s」など
	continue=続きから録画
ts-end
録画終了時間 [時間]h[分]m[秒]s形式で指定　例 12h52m3sなど


動作環境
--------
.NET 4.5.2以上


ライセンス
--------
SnkLib.App.CookieGetter
https://github.com/namoshika/SnkLib.App.CookieGetter/
Copyright (c) 2014 namoshika様.
Released under the GNU Lesser GPL
https://github.com/namoshika/SnkLib.App.CookieGetter/blob/master/LICENSE.txt

CookieGetterSharp
https://github.com/namoshika/CookieGetterSharp
Copyright (c) 2014 halxxxx様、うつろ様、にょんにょん様、炬燵犬様, namoshika様
Released under the GNU Lesser GPL

WebSocket4Net
https://github.com/kerryjiang/WebSocket4Net
Copyright (c) 2012 Kerry Jiang様
licensed under the Apache License 2.0
https://github.com/kerryjiang/WebSocket4Net/blob/master/LICENSE.TXT

SuperSocket
https://github.com/kerryjiang/SuperSocket/
Copyright (c) 2012 Kerry Jiang様
licensed under the Apache License 2.0
https://github.com/kerryjiang/SuperSocket/blob/master/LICENSE

Json.NET
https://github.com/JamesNK/Newtonsoft.Json
Copyright (c) 2007 James Newton-King
licensed under the MIT License
https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md

RTMPDump
https://rtmpdump.mplayerhq.hu/
Copyright (C) 2009 Andrej Stepanchuk
Copyright (C) 2010-2011 Howard Chu
The Flvstreamer Team
License: GPLv2

Bouncy Castle
http://www.bouncycastle.org/
Copyright (c) 2000 - 2020 The Legion of the Bouncy Castle Inc.
MIT license
http://www.bouncycastle.org/licence.html


更新
----
ver0.1.11 呼び出された際に自動で放送に接続できなくなっていた不具合を修正
ver0.1.10 前回の終了時の位置を保存する機能を追加
ver0.1.9 最小化時に通知領域にのみ表示する設定を追加、最小化の状態で起動する設定を追加。
ver0.1.8 ver0.1.7で過去のバージョンから設定を引き継いだ場合にウィンドウが正常に表示されなくなっていたのを修正。最大化ボタンが無効になるように修正。通知領域から画質と遅延を設定できる機能を追加。(2020/07/09)
ver0.1.7 遅延の設定を追加(2020/06/18)
ver0.1.6 放送情報を取得するAPIの仕様変更に対応(2020/06/03)
ver0.1.5 再接続時に画質変更前の画質に戻ることがある不具合を修正、フォームと文字の色の設定を追加(2020/02/24)
ver0.1.4 クッキー修正版 Chromiumベース版Microsoft Edgeのクッキーに対応(2020/02/16)
ver0.1.4 Chromeのクッキー取得修正の差し替え版 Chromeのクッキーが取得できなくなっていたのを修正(2020/02/14)
ver0.1.4 録画モードで開始されることがある不具合を修正(2019/06/30)
ver0.1.3 RTMPを使用したリアルタイム録画の取得元を変更(2019/05/11)
ver0.1.2 起動と同時に開始、視聴後に終了するように修正(2019/01/31)
ver0.1.1 視聴中の画質選択機能の追加、視聴中に止まることがある不具合の修正(2019/01/30)
ver0.1.0 (2018/12/26)

開発
-----
http://com.nicovideo.jp/community/co2414037
https://github.com/guest-nico/ncvPlayPlugin