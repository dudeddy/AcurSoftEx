Imports AcurSoft.Data
Imports AcurSoft.XtraGrid.Views.Grid.Extenders
Imports DevExpress.Data
Imports DevExpress.Utils.Serializing
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Grid
Imports System.ComponentModel

Namespace AcurSoft.XtraGrid

    Public Class GridColumnSummaryItemEx
        Inherits GridColumnSummaryItem
        Private _SummaryTypeEx As SummaryItemTypeEx2
        <XtraSerializableProperty, XtraSerializablePropertyId(2)>
        Public Property SummaryTypeEx As SummaryItemTypeEx2
            Get
                Return _SummaryTypeEx
            End Get
            Set(value As SummaryItemTypeEx2)
                If _SummaryTypeEx = value Then Return
                _SummaryTypeEx = value
                Me.SummaryType = Me.FixSummaryType(value)
                'If value <> SummaryItemTypeEx2.None AndAlso Me.View IsNot Nothing Then
                '    Me.View.OptionsView.ShowFooter = True
                'End If
                Me.OnChanged()
            End Set
        End Property

        Private _Format As IFormatProvider
        Public Shadows Property Format As IFormatProvider
            Get
                If _Format Is Nothing Then
                    Me.FixFormat()
                End If
                Return _Format
            End Get
            Set(value As IFormatProvider)
                If _Format Is value Then Return
                _Format = value
                Me.OnChanged()
            End Set
        End Property

        'Overloads Sub OnInsert(index As Integer, item As Object)

        'End Sub

        Public Sub FixFormat()
            If SummaryItemTypeHelperEx.IsSumOrAvg(Me.SummaryTypeEx) AndAlso Me.IsTimeSpan Then
                _Format = New TimeSpanFormatHelper(Me.DisplayFormat)
            Else
                _Format = MyBase.Format
            End If
            Me.OnChanged()
        End Sub

        Public ReadOnly Property IsTimeSpan As Boolean
            Get
                Return Me.SummaryColumn IsNot Nothing AndAlso Me.SummaryColumn.ColumnType Is GetType(TimeSpan)
            End Get
        End Property


        Public ReadOnly Property SummaryColumn As GridColumn
            Get
                If Me.Column Is Nothing Then Return Nothing
                Return Me.Column.View.Columns(Me.FieldName)
            End Get
        End Property

        Public ReadOnly Property View As GridView
            Get
                Return DirectCast(Me.Column?.View, GridView)
            End Get
        End Property


        Public Sub New()
            MyBase.New()
            _SummaryTypeEx = SummaryItemTypeEx2.None
        End Sub
#Region "Base Constructor"

        Public Sub New(summaryType As SummaryItemType)
            MyBase.New(summaryType)
            Me.FixSummaryTypeEx(summaryType)
        End Sub

        Public Sub New(summaryType As SummaryItemType, fieldName As String, displayFormat As String)
            MyBase.New(summaryType, fieldName, displayFormat)
            Me.FixSummaryTypeEx(summaryType)
        End Sub
        Public Sub New(summaryType As SummaryItemType, fieldName As String, displayFormat As String, tag As Object)
            MyBase.New(summaryType, fieldName, displayFormat, tag)
            Me.FixSummaryTypeEx(summaryType)
        End Sub

#End Region

#Region "Ex Constructor"
        Public Sub New(summaryTypeEx As SummaryItemTypeEx2)
            MyBase.New(If(summaryTypeEx.value__ < 100, DirectCast(summaryTypeEx.value__, SummaryItemType), SummaryItemType.Custom))
            _SummaryTypeEx = summaryTypeEx
        End Sub

        Public Sub New(summaryTypeEx As SummaryItemTypeEx2, fieldName As String, displayFormat As String)
            MyBase.New(If(summaryTypeEx.value__ < 100, DirectCast(summaryTypeEx.value__, SummaryItemType), SummaryItemType.Custom), fieldName, displayFormat)
            _SummaryTypeEx = summaryTypeEx
        End Sub

        Public Sub New(summaryTypeEx As SummaryItemTypeEx2, fieldName As String, displayFormat As String, tag As Object)
            MyBase.New(If(summaryTypeEx.value__ < 100, DirectCast(summaryTypeEx.value__, SummaryItemType), SummaryItemType.Custom), fieldName, displayFormat, tag)
            _SummaryTypeEx = summaryTypeEx
            'Me.FixSummaryTypeEx(SummaryType)
        End Sub

        Public Sub New(view As GridView, summaryTypeEx As SummaryItemTypeEx2, fieldName As String, Optional displayFormat As String = Nothing, Optional info As Object = Nothing)
            MyBase.New(If(summaryTypeEx.value__ < 100, DirectCast(summaryTypeEx.value__, SummaryItemType), SummaryItemType.Custom), fieldName, displayFormat)
            _SummaryTypeEx = summaryTypeEx
            'Me.FixSummaryTypeEx(SummaryType)
            If String.IsNullOrEmpty(displayFormat) Then
                displayFormat = CustomSummaryHelper.GetSummaryTypeDisplayFormat(summaryTypeEx, view.Columns(fieldName))
            End If
            Me.DisplayFormat = displayFormat
            Me.FixFormat()
            If info Is Nothing Then
                Me.Info = Me.FixSummaryInfo()
            End If
        End Sub

        Public Shared Function CreateInstance(col As GridColumn, summaryTypeEx As SummaryItemTypeEx2, fieldName As String, Optional displayFormat As String = Nothing, Optional info As Object = Nothing, Optional clear As Boolean = False, Optional add As Boolean = False) As GridColumnSummaryItemEx
            Dim gv As GridView = DirectCast(col.View, GridView)
            If clear Then
                col.Summary.Clear()
            End If
            Dim gsi As New GridColumnSummaryItemEx(gv, summaryTypeEx, fieldName, displayFormat, info)
            If add Then
                col.Summary.Add(gsi)
            End If
            If gsi.SummaryType <> SummaryItemType.None Then
                gv.OptionsView.ShowFooter = True
                gsi.CleanCollection()
            End If
            Return gsi
        End Function


        'Public Sub New(col As GridColumn, field As String, st As SummaryItemTypeEx2, Optional displayFormat As String = Nothing, Optional info As Object = Nothing)
        '    Dim gv As GridView = DirectCast(col.View, GridView)
        '    Dim c As GridColumn = gv.Columns(field)
        '    Dim gsi As GridColumnSummaryItem = Nothing
        '    If st = SummaryItemTypeEx2.None AndAlso col.Summary.Count >= 1 Then
        '        Me.New(SummaryItemType.None, field, Nothing, Nothing)
        '        gsi = CustomSummaryHelper.GetSummaryItem(c, SummaryItemTypeEx2.None)
        '    Else
        '        gsi = CustomSummaryHelper.GetSummaryItem(c, st, displayFormat, info)
        '    End If
        '    Return gsi
        'End Sub
#End Region

        'Public Overrides Function GetDefaultDisplayFormat() As String
        '    If Me.IsStandard Then
        '        MyBase.GetDefaultDisplayFormat()
        '    End If
        'End Function

        Public Sub FixSummaryTypeEx(summaryType As SummaryItemType)
            If summaryType <> SummaryItemType.Custom Then
                _SummaryTypeEx = DirectCast(summaryType.value__, SummaryItemTypeEx2)
            End If
        End Sub

        Public Function FixSummaryType(summaryTypeEx As SummaryItemTypeEx2) As SummaryItemType
            If summaryTypeEx.value__ <= 100 Then
                Return DirectCast(summaryTypeEx.value__, SummaryItemType)
            End If
            'Me.Column.View.c
            Return SummaryItemType.Custom
        End Function

        Public Shared Function FixSummaryInfo(st As SummaryItemTypeEx2, Optional info As Object = Nothing) As Object
            If info Is Nothing Then
                If SummaryItemTypeHelperEx.IsPercent(st) Then
                    info = 50
                ElseIf SummaryItemTypeHelperEx.IsTopButtom(st)
                    info = 5
                End If
            End If
            Return info
        End Function


        Private _Info As Object
        <XtraSerializableProperty(), System.ComponentModel.DefaultValue(CType(Nothing, Object)), Editor(GetType(DevExpress.Utils.Editors.UIObjectEditor), GetType(System.Drawing.Design.UITypeEditor)), TypeConverter(GetType(DevExpress.Utils.Editors.ObjectEditorTypeConverter))>
        Public Property Info As Object
            Get
                _Info = FixSummaryInfo(Me.SummaryTypeEx, _Info)
                Return _Info
            End Get
            Set(value As Object)
                If _Info Is value Then Return
                _Info = FixSummaryInfo(Me.SummaryTypeEx, value)
                Me.OnChanged()
            End Set
        End Property

        Public Function FixSummaryInfo() As Object
            If SummaryItemTypeHelperEx.IsPercent(Me.SummaryTypeEx) Then
                Return 50
            ElseIf SummaryItemTypeHelperEx.IsTopButtom(Me.SummaryTypeEx)
                Return 5
            End If
            Return Nothing
        End Function

        Public Overrides Sub Assign(source As GridSummaryItem)
            MyBase.Assign(source)
            If TypeOf source Is GridColumnSummaryItemEx Then
                With DirectCast(source, GridColumnSummaryItemEx)
                    Me.SummaryTypeEx = .SummaryTypeEx
                    Me.Info = .Info
                    Me.Format = .Format
                End With
            End If
            Me.OnChanged()
        End Sub

        Public Sub ReAssign(field As String, st As SummaryItemTypeEx2, Optional displayFormat As String = Nothing, Optional info As Object = Nothing)
            Me.FieldName = field
            Me.DisplayFormat = displayFormat
            Me.SummaryTypeEx = st
            Me.SummaryType = Me.FixSummaryType(st)
            Me.Info = info
            Me.FixFormat()
        End Sub


        Public Sub CleanCollection()
            While Me.Column.Summary.Any(Function(q) q.SummaryType = SummaryItemType.None)
                Dim toDelGsi As GridSummaryItem = Me.Column.Summary.FirstOrDefault(Function(q) q.SummaryType = SummaryItemType.None)
                If toDelGsi Is Nothing Then Continue While
                Me.Column.Summary.Remove(toDelGsi)
            End While
        End Sub


    End Class
End Namespace
