Imports System.ComponentModel
Imports System.Reflection
Imports AcurSoft.XtraGrid.Columns
Imports AcurSoft.XtraGrid.Columns.Helpers
Imports AcurSoft.XtraGrid.Views.Grid
Imports AcurSoft.XtraGrid.Views.Grid.Extenders
Imports AcurSoft.XtraGrid.Views.Grid.Summary
Imports DevExpress.Utils.Drawing
Imports DevExpress.Utils.Serializing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Columns

Namespace AcurSoft.XtraGrid.Columns

    Public Class GridExColumn
        Inherits GridColumn
        Private _CheckedStateRepository As ColumnStateRepository
        Public Property CheckedStateRepository As ColumnStateRepository
            Get
                If _CheckedStateRepository Is Nothing Then
                    _CheckedStateRepository = New ColumnStateRepository With {.State = ObjectState.Normal, .Checked = False}
                End If
                Return _CheckedStateRepository
            End Get
            Set(value As ColumnStateRepository)
                _CheckedStateRepository = value
            End Set
        End Property

        Private _FillEmptySpace As Boolean = False
        <Browsable(False)>
        Public Property FillEmptySpace() As Boolean
            Get
                Return _FillEmptySpace
            End Get
            Set(ByVal value As Boolean)
                SetFillEmptySpace(value)
            End Set
        End Property

        Public Sub New()
            MyBase.New()
            _Summary = New GridColumnSummaryItemCollectionEx(Me)
            Dim fi As FieldInfo = GetType(GridColumn).GetField("summary", BindingFlags.NonPublic Or BindingFlags.Instance)
            fi.SetValue(Me, _Summary)
            AddHandler _Summary.CollectionChanged, AddressOf Summary_CollectionChanged
        End Sub

        Private Sub Summary_CollectionChanged(sender As Object, e As CollectionChangeEventArgs)
            If Me.View Is Nothing Then Return
            DirectCast(Me.View, GridViewEx).OnColumnSummaryCollectionChangedEx(Me, e)

        End Sub

        Public Shadows ReadOnly Property OptionsFilter() As OptionsColumnFilterEx
            Get
                Return TryCast(MyBase.OptionsFilter, OptionsColumnFilterEx)
            End Get
        End Property
        Protected Overrides Function CreateOptionsFilter() As OptionsColumnFilter
            Return New OptionsColumnFilterEx()
        End Function

        Public Overrides Sub BestFit()
            MyBase.BestFit()
            If DirectCast(Me.View, GridViewEx).OptionsBehavior.ColumnsSelectionHelper.DrawCheckBoxMode <> DrawCheckBoxModeEnum.Never Then
                Me.Width += GridViewColumnsSelectionHelper.CheckBoxWidth
            End If
        End Sub


        Protected Overrides Function GetFilterPopupMode() As FilterPopupMode
            Dim modeExtended As FilterPopupModeExtended = OptionsFilter.FilterPopupMode
            Dim mode As FilterPopupMode = FilterPopupMode.List
            If modeExtended = FilterPopupModeExtended.Default AndAlso (ColumnType.Equals(GetType(Date)) OrElse ColumnType.Equals(GetType(Date?))) AndAlso FilterMode <> ColumnFilterMode.DisplayText Then
                modeExtended = FilterPopupModeExtended.DateSmart
            End If
            If modeExtended = FilterPopupModeExtended.Default Then
                modeExtended = FilterPopupModeExtended.List
            End If
            OptionsFilter.UseFilterPopupRangeDateMode = False
            Select Case modeExtended.ToString()
                Case "Default"
                    mode = FilterPopupMode.Default
                    Exit Select
                Case "List"
                    mode = FilterPopupMode.List
                    Exit Select
                Case "CheckedList"
                    mode = FilterPopupMode.CheckedList
                    Exit Select
                Case "Date"
                    mode = FilterPopupMode.Date
                    Exit Select
                Case "DateSmart"
                    mode = FilterPopupMode.DateSmart
                    Exit Select
                Case "DateAlt"
                    mode = FilterPopupMode.DateAlt
                    Exit Select
                Case "DateRange"
                    mode = FilterPopupMode.Date
                    OptionsFilter.UseFilterPopupRangeDateMode = True
                    Exit Select
                Case Else
            End Select
            Return mode
        End Function

        Protected Overridable Sub SetFillEmptySpace(ByVal value As Boolean)
            _FillEmptySpace = value
            If IsLoading OrElse View Is Nothing Then Return
            If _FillEmptySpace Then
                Me.Width += 1
                Dim oldCol As GridExColumn = Me.View.Columns.OfType(Of GridExColumn).FirstOrDefault(Function(q) q IsNot Me AndAlso q.FillEmptySpace)
                If oldCol IsNot Nothing Then
                    oldCol.FillEmptySpace = False
                    oldCol.BestFit()
                End If
            Else
                'Me.BestFit()
            End If
        End Sub

        Private _Summary As GridColumnSummaryItemCollectionEx
        <Localizable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), XtraSerializableProperty(True, True, True, 1000), XtraSerializablePropertyId(LayoutIdData), DXCategory(CategoryName.Data)>
        Public Shadows ReadOnly Property Summary As GridColumnSummaryItemCollectionEx
            Get
                Return TryCast(MyBase.Summary, GridColumnSummaryItemCollectionEx)
            End Get
        End Property
    End Class
End Namespace
