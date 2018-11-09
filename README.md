# MyKeyChangerForDebug
デバッグ用のキーボードショートカットキーを一元化するためのキーマッピング変更ツール。厳密にはキーマッピング変更というよりはマクロの割当が近いかと思います。

![key assign](https://user-images.githubusercontent.com/31182578/48263691-55166f00-e46a-11e8-841d-223b66bcfc5a.png)

## 2018.11.09
実装は完了していますが軽い単体を行っただけなのでチョンボはあるかもです。また、以前キーマッピングの変更ツールを作成した時にも遭遇したのですが、キー入力がおかしくなる可能性があります。その場合はPCの再起動が必要になるかも知れません。
※このケースはKeyUPのイベントをOSに通知できずキーを押しっぱなしと認識されている状態。

キーマッピングの設定を変更する場合は KeymappingHandler で定義しているテンプレート(80行目付近)の Dictionary を修正します。

キーを順番に押していき、最後に押したキーから離す場合はKeyDataの第２パラメータはFlags.Noneを指定してください。
例) Ctrl + C
```
KeyData(KeySet.ControlL, Flags.None)
KeyData(KeySet.C, Flags.None)
```
この場合は CtrlのKeydown → CのKeyDown → CのKeyUp → CtrlのKeyUpを実行します。

修飾キーを押したまま他のキーを複数押下する場合はKeyUp、KeyDownを全て指定してください。
例）Ctrl + K, Ctrl + V(Ctrlキーを押したままK、Vの順番に押下)
```
KeyData(KeySet.ControlL, Flags.KeyDown)
KeyData(KeySet.K, Flags.KeyDown)
KeyData(KeySet.K, Flags.KeyUp)
KeyData(KeySet.V, Flags.KeyDown)
KeyData(KeySet.V, Flags.KeyUp)
KeyData(KeySet.ControlL, Flags.KeyUp)
```
この場合はCtrlのKeyDown → KのKeyDown → KのKeyUp → VのKeyDown → VのKeyUp → CtrlのKeyUpを実行します。
KeyUpを忘れるとOSにはキーが押しっぱなしと認識されるのでご注意ください。
