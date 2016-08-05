Imports AcurSoft.XtraEditors.Base
Imports AcurSoft.XtraEditors.Repository
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors.Drawing
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors.ViewInfo

Namespace AcurSoft.XtraEditors.ViewInfo
    Public Class SpinButtonEditExViewInfo
        'Inherits DateEditViewInfo
        Inherits BaseSpinEditViewInfo
        Public Sub New(ByVal item As RepositoryItem)
            MyBase.New(item)
        End Sub

        Public Shadows ReadOnly Property Item() As RepositoryItemPopupBase
            Get
                Return TryCast(Owner, RepositoryItemPopupBase)
            End Get
        End Property

        Protected Overrides Function CreateButtonInfo(ByVal button As DevExpress.XtraEditors.Controls.EditorButton, ByVal index As Integer) As DevExpress.XtraEditors.Drawing.EditorButtonObjectInfoArgs
            'Item.SpinButtonIndex
            If index = DirectCast(Item, ISpinButtonEditor).SpinButtonIndex Then
                Return New SpinButtonObjectInfoArgs(Me, button, PaintAppearance, SpinStyles.Vertical)
            End If
            Return New EditorButtonObjectInfoArgs(button, PaintAppearance)
        End Function

        Protected Overrides Function CalcButtonState(ByVal info As EditorButtonObjectInfoArgs, ByVal index As Integer) As Boolean
            Dim ee As SpinButtonObjectInfoArgs = TryCast(info, SpinButtonObjectInfoArgs)
            If ee Is Nothing Then
                Return MyBase.CalcButtonState(info, index)
            End If
            Dim prevUpState As ObjectState = ee.UpButtonInfo.State, prevDownState As ObjectState = ee.DownButtonInfo.State, newUpState As ObjectState = ObjectState.Normal, newDownState As ObjectState = ObjectState.Normal
            If State = ObjectState.Disabled Then
                ee.State = ObjectState.Disabled
                ee.DownButtonInfo.State = ee.State
                ee.UpButtonInfo.State = ee.DownButtonInfo.State
                Return prevUpState <> ee.UpButtonInfo.State OrElse prevDownState <> ee.DownButtonInfo.State
            End If
            Dim hotButton As EditorButtonObjectInfoArgs = TryCast(HotInfo.HitObject, EditorButtonObjectInfoArgs)
            If PressedInfo.HitTest = EditHitTest.Button Then
                Dim press As EditorButtonObjectInfoArgs = TryCast(PressedInfo.HitObject, EditorButtonObjectInfoArgs)
                Dim pressedSpin As EditorButtonObjectInfoArgs = Nothing
                If IsEqualsButtons(press, ee.UpButtonInfo) Then
                    pressedSpin = ee.UpButtonInfo
                End If
                If IsEqualsButtons(press, ee.DownButtonInfo) Then
                    pressedSpin = ee.DownButtonInfo
                End If
                If pressedSpin IsNot Nothing Then
                    Dim st As ObjectState = ObjectState.Pressed
                    If IsEqualsButtons(pressedSpin, hotButton) Then
                        st = st Or ObjectState.Hot
                    End If
                    If pressedSpin Is ee.DownButtonInfo Then
                        newDownState = st
                    Else
                        newUpState = st
                    End If
                End If
            Else
                If IsEqualsButtons(hotButton, ee.UpButtonInfo) Then
                    newUpState = ObjectState.Hot
                Else
                    If IsEqualsButtons(hotButton, ee.DownButtonInfo) Then
                        newDownState = ObjectState.Hot
                    End If
                End If
            End If
            If prevUpState <> newUpState Then
                OnBeforeButtonStateChanged(ee.UpButtonInfo, newUpState, -1000)
            End If
            If prevDownState <> newDownState Then
                OnBeforeButtonStateChanged(ee.DownButtonInfo, newDownState, -1001)
            End If
            ee.UpButtonInfo.State = newUpState
            ee.DownButtonInfo.State = newDownState
            Return prevUpState <> ee.UpButtonInfo.State OrElse prevDownState <> ee.DownButtonInfo.State
        End Function

        Private Function IsEqualsButtons(ByVal b1 As EditorButtonObjectInfoArgs, ByVal b2 As EditorButtonObjectInfoArgs) As Boolean
            If b1 Is Nothing OrElse b2 Is Nothing Then
                Return False
            End If
            Return b1.Button.Kind = b2.Button.Kind
        End Function

    End Class
End Namespace
