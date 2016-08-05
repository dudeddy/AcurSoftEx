Imports DevExpress.Utils.Controls
Imports DevExpress.XtraGrid.Columns
Namespace AcurSoft.XtraGrid.Columns

    Public Class OptionsColumnFilterEx
        Inherits OptionsColumnFilter

        Protected Friend UseFilterPopupRangeDateMode As Boolean = False

        Private _FilterPopupMode As FilterPopupModeExtended
        Public Shadows Property FilterPopupMode() As FilterPopupModeExtended
            Get
                Return _FilterPopupMode
            End Get
            Set(ByVal value As FilterPopupModeExtended)
                If _FilterPopupMode = value Then Return
                Dim prevValue As FilterPopupModeExtended = _FilterPopupMode
                _FilterPopupMode = value
                OnChanged(New BaseOptionChangedEventArgs("FilterPopupMode", prevValue, value))
            End Set
        End Property

    End Class
End Namespace

