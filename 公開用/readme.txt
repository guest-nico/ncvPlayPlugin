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


動作環境
--------
.NET 4.5.2以上


ライセンス
--------
SnkLib.App.CookieGetter
https://github.com/namoshika/SnkLib.App.CookieGetter
namoshika様

CookieGetterSharp
https://github.com/namoshika/CookieGetterSharp
halxxxx様、うつろ様、にょんにょん様、炬燵犬様

WebSocket4Net
https://github.com/kerryjiang/WebSocket4Net
kerryjiang様

Json.NET
https://www.newtonsoft.com/json


更新
----
ver0.1.7 遅延の設定を追加
ver0.1.6 放送情報を取得するAPIの仕様変更に対応
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