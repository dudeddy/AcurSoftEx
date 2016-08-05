Imports System.ComponentModel
Imports DevExpress.Utils.Controls
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Namespace AcurSoft.XtraGrid.Views.Grid

    Public Class GridViewOptionsFindEx
        Inherits GridViewOptionsFind

        <Browsable(False)>
        Public ReadOnly Property ViewEx As GridViewEx

        Private _FindBackGroundColor As Color
        Public Property FindBackGroundColor() As Color
            Get
                Return _FindBackGroundColor
            End Get
            Set(value As Color)
                If value = _FindBackGroundColor Then Return
                Dim prevValue As Color = _FindBackGroundColor
                _FindBackGroundColor = value
                OnChanged(New BaseOptionChangedEventArgs("FindBackGroundColor", prevValue, value))
            End Set
        End Property

        Private _FindHighLightColor As Color
        Public Property FindHighLightColor() As Color
            Get
                Return _FindHighLightColor
            End Get
            Set(value As Color)
                If value = _FindHighLightColor Then Return
                Dim prevValue As Color = _FindHighLightColor
                _FindHighLightColor = value
                OnChanged(New BaseOptionChangedEventArgs("FindHighLightColor", prevValue, value))
            End Set
        End Property


        Private _FindFilterColumns As String
        Public Overrides Property FindFilterColumns As String
            Get
                If String.IsNullOrEmpty(_FindFilterColumns) Then
                    _FindFilterColumns = "*"
                End If
                Return _FindFilterColumns
            End Get
            Set(value As String)
                If _FindFilterColumns = value Then Return
                Dim prevValue As String = _FindFilterColumns
                If String.IsNullOrEmpty(value) Then
                    value = "*"
                ElseIf value.Contains(",") OrElse value.Contains(" ")
                    value = value.Replace(", ", ";").Replace(",", ";").Replace(" ", "")
                End If
                MyBase.FindFilterColumns = value
                _FindFilterColumns = value
                OnChanged(New BaseOptionChangedEventArgs("FindFilterColumns", prevValue, value))
            End Set
        End Property


        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(viewEx As GridViewEx)
            MyBase.New()
            _ViewEx = viewEx
            _FindBackGroundColor = Color.Green
            _FindHighLightColor = Color.Gold
        End Sub
    End Class
End Namespace
