Imports AcurSoft.Data.Filtering.Helpers
Imports AcurSoft.XtraFilterEditor
Imports AcurSoft.XtraGrid.Columns
Imports AcurSoft.XtraGrid.Controls
Imports AcurSoft.XtraGrid.Views.Base
Imports AcurSoft.XtraGrid.Views.Grid.Extenders

Imports DevExpress.Data
Imports DevExpress.Data.Filtering
Imports DevExpress.Data.Helpers
Imports DevExpress.Data.Summary
Imports DevExpress.Utils.Drawing

Imports DevExpress.Utils.Menu
Imports DevExpress.Utils.Serializing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors.Drawing
Imports DevExpress.XtraGrid

Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Drawing
Imports DevExpress.XtraGrid.Scrolling
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports System.ComponentModel
Imports AcurSoft.Data
'How to add custom filter items into the DateTime filter popup window
'http://www.devexpress.com/example=E4265
'GridView - How to add a custom button to the FilterPanel
'https://www.devexpress.com/Support/Center/Example/Details/T375271

'Shows how to add the bookmark functionality [TODO: Add bookmark functionality]
'https://www.devexpress.com/Support/Center/Example/Details/E1267
Namespace AcurSoft.XtraGrid.Views.Grid

    Public Class GridViewEx
        Inherits GridView
        Public Const VIEW_NAME As String = "GridViewEx"


        Private _FilterPanelButtonsStore As Dictionary(Of EditorButton, EditorButtonObjectInfoArgs)



        Private _NeedColumnRecalc As Boolean = False
        Private _OptionsFind As GridViewOptionsFindEx

        Public Sub New()
            MyBase.New()
            RemoveHandler Me.ColumnPositionChanged, AddressOf OnColumnPositionChanged
            AddHandler Me.ColumnPositionChanged, AddressOf OnColumnPositionChanged
            'FilterPanelButtonsStore = New Dictionary(Of EditorButton, EditorButtonObjectInfoArgs)()
        End Sub

        'Overrides bes

        Public Sub New(ByVal grid As GridControl)
            MyBase.New(grid)
        End Sub

        Public Event CustomFilterPanelButtonClick As EventHandler

        Private Sub OnColumnPositionChanged(sender As Object, e As EventArgs)
            If Not Me.OptionsView.FillEmptySpace Then Return
            If sender Is Nothing Then Return
            Dim lastCol As GridExColumn = Me.Columns.LastVisibleColumn
            If lastCol IsNot Nothing Then
                lastCol.FillEmptySpace = True
            End If
        End Sub

        Protected Overrides Function ConvertGridFilterToDataFilter(ByVal criteria As CriteriaOperator) As CriteriaOperator
            If Not String.IsNullOrEmpty(FindFilterText) Then
                _FindColumns = Me.FindControl.GetCheckedColumns()
                Dim lastParserResults As FindSearchParserResults = New FindSearchParser().Parse(FindFilterText, _FindColumns)
                Dim findCriteria As CriteriaOperator = CustomCriteriaHelper.Create(lastParserResults, FilterCondition.Contains, IsServerMode)
                If Me.OptionsFilter.AccentInsensitive Then
                    findCriteria = GridFilterAccentSubstitutor.Substitute(findCriteria)
                End If
                Return criteria And findCriteria
            End If
            Return MyBase.ConvertGridFilterToDataFilter(criteria)
        End Function

        Protected Overrides Function CreateColumnCollection() As GridColumnCollection
            Return New GridExColumnCollection(Me)
        End Function

        Protected Overrides Function CreateDateFilterPopup(ByVal column As GridColumn, ByVal ownerControl As System.Windows.Forms.Control, ByVal creator As Object) As DateFilterPopup
            Return New DateFilterPopupEx(Me, column, ownerControl, creator)
        End Function
        Protected Overrides Function CreateFindPanel(ByVal findPanelProperties As Object) As DevExpress.XtraGrid.Controls.FindControl
            _FindControl = New FindControlEx(Me, findPanelProperties)
            Me.RequestXtraPanelControl()

            Return FindControl
        End Function

        Protected Overrides Function CreateOptionsFilter() As ColumnViewOptionsFilter
            Return New GridViewOptionsFilterEx(Me)
        End Function
        Protected Overrides Function CreateOptionsFind() As ColumnViewOptionsFind
            _OptionsFind = New GridViewOptionsFindEx(Me)
            Return _OptionsFind
        End Function

        Protected Overrides Sub OnDataManager_CustomSummaryEvent(sender As Object, e As CustomSummaryEventArgs)
            MyBase.OnDataManager_CustomSummaryEvent(sender, e)
            Extenders.CustomSummaryHelper.GetUniqueValuesCount(Me, e)
            Extenders.CustomSummaryHelper.GetTopBottomSummary(Me, e)
        End Sub

        Protected Overrides Sub OnFilterPopupValuesReady(column As GridColumn, values() As Object)
            MyBase.OnFilterPopupValuesReady(column, values)
        End Sub


        Protected Overrides Sub RaiseCustomDrawColumnHeader(e As EventArgs)
            MyBase.RaiseCustomDrawColumnHeader(e)
            Me.ShowSortIndexInColumnHeader(TryCast(e, ColumnHeaderCustomDrawEventArgs))
        End Sub

        Protected Overrides Sub RaiseFilterPopupDate(ByVal filterPopup As DateFilterPopup, ByVal list As List(Of FilterDateElement))
            Dim filter As CriteriaOperator = New BinaryOperator(filterPopup.Column.FieldName, Date.Today, BinaryOperatorType.Greater)

            list.Add(New FilterDateElement(Localizer.Active.GetLocalizedString(StringId.FilterClauseGreater), String.Empty, filter))
            filter = New BinaryOperator(filterPopup.Column.FieldName, Date.Today, BinaryOperatorType.Less)
            list.Add(New FilterDateElement(Localizer.Active.GetLocalizedString(StringId.FilterClauseLess), String.Empty, filter))
            filter = New BetweenOperator(filterPopup.Column.FieldName, Date.Today, Date.Today)
            list.Add(New FilterDateElement(Localizer.Active.GetLocalizedString(StringId.FilterClauseBetween), String.Empty, filter))
            MyBase.RaiseFilterPopupDate(filterPopup, list)
        End Sub

        'Overrides summar


        Protected Overrides Sub RaisePopupMenuShowing(e As PopupMenuShowingEventArgs)
            MyBase.RaisePopupMenuShowing(e)
            If e.MenuType = GridMenuType.Column AndAlso e.HitInfo.Column IsNot Nothing Then
                Dim msiSummaries As New DXSubMenuItem("Summaries") With {.BeginGroup = True}
                e.Menu.Items.Add(msiSummaries)
                'Dim miSummaryConfigDialog As DXMenuItem = Me.GetSummaryConfigDialogMenuItem(e)
                msiSummaries.Items.Add(Me.GetSummaryConfigDialogMenuItem(e))
                Dim msiAddSummaries As New DXSubMenuItem("Add Summaries") With {.BeginGroup = True}
                msiSummaries.Items.Add(msiAddSummaries)
                Me.BuildSummariesMenuItems(e.HitInfo.Column, Nothing, msiAddSummaries.Items, True)
                If e.HitInfo.Column.Summary.ActiveCount > 0 Then
                    Dim miClear As DXMenuItem = CreateSummaryColumnMenuItem(SummaryItemTypeEx2.None, e.HitInfo.Column, Nothing, True)
                    miClear.Caption = "Clear Summaries"
                    msiSummaries.Items.Add(miClear)
                End If

            ElseIf e.MenuType = GridMenuType.Summary AndAlso e.HitInfo.Column IsNot Nothing Then
                e.Menu.Items.Clear()
                e.Menu.Items.Add(Me.GetSummaryConfigDialogMenuItem(e))
                Dim msiAddSummaries As New DXSubMenuItem("Add Summaries") With {.BeginGroup = True}
                e.Menu.Items.Add(msiAddSummaries)
                Me.BuildSummariesMenuItems(e.HitInfo.Column, Nothing, msiAddSummaries.Items, True)

                Dim si As GridColumnSummaryItem = DirectCast(DirectCast(e.Menu, DevExpress.XtraGrid.Menu.GridViewFooterMenu).SummaryItem, GridColumnSummaryItem)
                If si IsNot Nothing Then
                    e.Menu.Items.Add(GetSummaryEditDialogMenuItem(e))
                    Me.BuildSummariesMenuItems(e.HitInfo.Column, si, e.Menu.Items, False, True)
                    If e.HitInfo.Column.Summary.Count = 1 AndAlso e.HitInfo.Column.SummaryItem.SummaryType = SummaryItemType.None Then
                    Else
                        e.Menu.Items.Add(CreateSummaryColumnMenuItem(SummaryItemTypeEx2.None, e.HitInfo.Column, si, True))
                        Dim miClear As DXMenuItem = CreateSummaryColumnMenuItem(SummaryItemTypeEx2.None, e.HitInfo.Column, Nothing, True)
                        miClear.Caption = "Clear Summaries"
                        e.Menu.Items.Add(miClear)
                    End If
                End If


            End If
        End Sub

        Private Sub BuildSummariesMenuItems(col As GridColumn, orgSummaryItem As GridColumnSummaryItem, msiSummariesItems As DXMenuItemCollection, add As Boolean, Optional beginGroup As Boolean = False)
            msiSummariesItems.Add(CreateSummaryColumnMenuItem(SummaryItemTypeEx2.Count, col, orgSummaryItem, beginGroup, add))
            msiSummariesItems.Add(CreateSummaryColumnMenuItem(SummaryItemTypeEx2.UniqueValuesCount, col, orgSummaryItem,, add))

            msiSummariesItems.Add(CreateSummaryColumnMenuItem(SummaryItemTypeEx2.Min, col, orgSummaryItem,, add))
            msiSummariesItems.Add(CreateSummaryColumnMenuItem(SummaryItemTypeEx2.Max, col, orgSummaryItem,, add))
            msiSummariesItems.Add(CreateSummaryColumnMenuItem(SummaryItemTypeEx2.Sum, col, orgSummaryItem,, add))
            msiSummariesItems.Add(CreateSummaryColumnMenuItem(SummaryItemTypeEx2.Average, col, orgSummaryItem,, add))
            Dim msiTop As New DXSubMenuItem("Top") With {.BeginGroup = True}
            msiTop.Items.Add(CreateSummaryColumnMenuItem(SummaryItemTypeEx2.TopXSum, col, orgSummaryItem,, add))
            msiTop.Items.Add(CreateSummaryColumnMenuItem(SummaryItemTypeEx2.TopXAvg, col, orgSummaryItem,, add))
            msiTop.Items.Add(CreateSummaryColumnMenuItem(SummaryItemTypeEx2.TopXPercentSum, col, orgSummaryItem,, add))
            msiTop.Items.Add(CreateSummaryColumnMenuItem(SummaryItemTypeEx2.TopXPercentAvg, col, orgSummaryItem,, add))
            msiSummariesItems.Add(msiTop)
            Dim msiBottom As New DXSubMenuItem("Bottom") With {.BeginGroup = True}
            msiBottom.Items.Add(CreateSummaryColumnMenuItem(SummaryItemTypeEx2.BottomXSum, col, orgSummaryItem,, add))
            msiBottom.Items.Add(CreateSummaryColumnMenuItem(SummaryItemTypeEx2.BottomXAvg, col, orgSummaryItem,, add))
            msiBottom.Items.Add(CreateSummaryColumnMenuItem(SummaryItemTypeEx2.BottomXPercentSum, col, orgSummaryItem,, add))
            msiBottom.Items.Add(CreateSummaryColumnMenuItem(SummaryItemTypeEx2.BottomXPercentAvg, col, orgSummaryItem,, add))
            msiSummariesItems.Add(msiBottom)
        End Sub

        Private Function GetSummaryEditDialogMenuItem(e As PopupMenuShowingEventArgs) As DXMenuItem
            Dim miSummaryEditDialog As New DXMenuItem With {.Caption = "Summary Edit", .BeginGroup = True}
            AddHandler miSummaryEditDialog.Click,
                Sub(s, a)
                    Dim si As GridColumnSummaryItem = Nothing
                    If e.MenuType = GridMenuType.Summary Then
                        si = DirectCast(DirectCast(e.Menu, DevExpress.XtraGrid.Menu.GridViewFooterMenu).SummaryItem, GridColumnSummaryItem)
                    ElseIf e.MenuType = GridMenuType.Column
                        Dim col As GridColumn = e.HitInfo.Column
                        If col.Summary.Count = 1 Then
                            si = DirectCast(col.SummaryItem, GridColumnSummaryItem)
                        End If
                    End If
                    If si IsNot Nothing Then
                        Using frm As New Extenders.ColumnSummaryConfig(e.HitInfo.Column, DirectCast(si, GridColumnSummaryItemEx))
                            If frm.ShowDialog = DialogResult.OK Then
                                frm.SaveChanges()
                            End If
                        End Using
                    End If
                End Sub

            Return miSummaryEditDialog
        End Function

        Private Function GetSummaryConfigDialogMenuItem(e As PopupMenuShowingEventArgs) As DXMenuItem
            Dim miSummaryConfigDialog As New DXMenuItem With {.Caption = "Summaries Config"}
            AddHandler miSummaryConfigDialog.Click,
            Sub(s, a)
                Using frm As New Extenders.ColumnSummariesForm(e.HitInfo.Column)
                    If frm.ShowDialog = DialogResult.OK Then
                        frm.SaveChanges()
                    End If
                End Using
            End Sub

            Return miSummaryConfigDialog
        End Function

        Protected Overridable Sub RecalculateColumnWidths()
            If Me.IsLoading Then Return
            Dim colToResize As GridExColumn = Nothing
            Dim totalWidth As Integer = 0

            Dim lastCol As GridExColumn = Me.Columns.LastVisibleColumn
            If lastCol IsNot Nothing Then
                lastCol.FillEmptySpace = True
            End If
            For i As Integer = 0 To Columns.Count - 1
                If Not Columns(i).Visible Then Continue For
                If Columns(i).FillEmptySpace Then
                    colToResize = Columns(i)
                Else
                    totalWidth += Columns(i).Width
                End If
            Next i

            If colToResize IsNot Nothing AndAlso ViewInfo.ViewRects.ColumnPanelWidth > 0 Then
                colToResize.Width = ViewInfo.ViewRects.ColumnPanelWidth - totalWidth
            End If
        End Sub

        Protected Overrides ReadOnly Property ViewName() As String
            Get
                Return GridViewEx.VIEW_NAME
            End Get
        End Property

        Friend Function UpdateFilterPanelButtonsRects() As Integer
            Dim offset As Integer = 10
            For Each button As EditorButton In FilterPanelButtonsStore.Keys
                Dim y As Integer = ViewInfo.ClientBounds.Y + (ViewInfo.ClientBounds.Height - ((ViewInfo.FilterPanel.Bounds.Height \ 8) * 7))
                Dim x As Integer = offset
                offset += 5 + button.Width
                Dim buttonRect As New Rectangle(x, y, button.Width, (ViewInfo.FilterPanel.Bounds.Height \ 8) * 6)
                FilterPanelButtonsStore(button).Bounds = buttonRect
            Next button
            Return offset
        End Function
        Friend Sub UpdateFilterPanelButtonState(ByVal state As ObjectState, ByVal point As Point)
            Dim hitInfo As GridHitInfo = CalcHitInfo(point)
            If hitInfo.HitTest <> GridHitTest.FilterPanel Then
                Return
            End If
            For Each button As EditorButton In FilterPanelButtonsStore.Keys
                button.Tag = ObjectState.Normal
                If FilterPanelButtonsStore(button).Bounds.Contains(point) Then
                    button.Tag = state
                    If state = ObjectState.Pressed Then
                        OnFilterPanelCustomButtonClick(button, EventArgs.Empty)
                    End If
                End If
            Next button
            InvalidateFilterPanel()
        End Sub

        Public Sub OnColumnSummaryCollectionChangedEx(column As GridColumn, e As CollectionChangeEventArgs)
            Me.OnColumnSummaryCollectionChanged(column, e)
        End Sub



        Public Sub AddFilterPanelButton(ByVal b As EditorButton)
            FilterPanelButtonsStore.Add(b, New EditorButtonObjectInfoArgs(b, New DevExpress.Utils.AppearanceObject()))
        End Sub
        Public Function CreateSummaryColumnMenuItem(st As SummaryItemTypeEx2, col As GridColumn, orgSummaryItem As GridColumnSummaryItem, Optional beginGroup As Boolean = False, Optional add As Boolean = False) As DXMenuCheckItem
            Dim mi As New DXMenuCheckItem() With {
                    .Caption = CustomSummaryHelper.GetSummaryTypeCaption(st, col),
                    .BeginGroup = beginGroup,
                    .Enabled = CustomSummaryHelper.CanApplySummary(st, col)
            }
            If Not add AndAlso st <> SummaryItemTypeEx2.None Then
                If orgSummaryItem Is Nothing Then
                    mi.Checked = DirectCast(col.SummaryItem, GridColumnSummaryItemEx).SummaryTypeEx = st
                Else
                    mi.Checked = DirectCast(orgSummaryItem, GridColumnSummaryItemEx).SummaryTypeEx = st
                End If
            End If

            AddHandler mi.CheckedChanged,
                    Sub(s, a)
                        If mi.Checked Then
                            Dim gv As GridView = DirectCast(col.View, GridView)
                            Dim c As GridColumn = gv.Columns(col.FieldName)
                            gv.BeginDataUpdate()
                            col.Summary.BeginUpdate()
                            If add Then
                                GridColumnSummaryItemEx.CreateInstance(col, st, col.FieldName, Nothing, Nothing, False, True)
                            Else
                                If orgSummaryItem Is Nothing Then
                                    GridColumnSummaryItemEx.CreateInstance(col, st, col.FieldName, Nothing, Nothing, True, True)
                                Else
                                    If st = SummaryItemTypeEx2.None AndAlso orgSummaryItem IsNot Nothing AndAlso orgSummaryItem.Collection.Count > 1 Then
                                        orgSummaryItem.Collection.Remove(orgSummaryItem)
                                    Else
                                        Dim gsi As New GridColumnSummaryItemEx(gv, st, col.FieldName, Nothing, Nothing)
                                        If gsi.SummaryType <> SummaryItemType.None Then
                                            gv.OptionsView.ShowFooter = True
                                        End If
                                        orgSummaryItem.Assign(gsi)
                                    End If
                                End If
                            End If
                            col.Summary.EndUpdate()
                            gv.EndDataUpdate()
                        End If
                    End Sub
            Return mi
        End Function

        Public Function GetCheckedColumns(Optional checked As Boolean = True) As IEnumerable(Of GridExColumn)
            Return Me.Columns.OfType(Of GridExColumn).Where(Function(q) q.CheckedStateRepository.Checked = checked)
        End Function

        Public Function GetFilteredValues(ByVal column As GridColumn, ByVal showAll As Boolean) As Object() ', ByVal completed As DevExpress.Data.OperationCompleted) As Object()
            Return MyBase.GetFilterPopupValues(column, showAll, Nothing)
        End Function

        Public Function GetFindColumns() As List(Of IDataColumnInfo)
            Return GetFindToColumnsCollection()
        End Function

        Public Overrides Sub LayoutChanged()
            MyBase.LayoutChanged()
            If Me.IsLoading Then Return
            If Not Me.OptionsView.FillEmptySpace Then Return
            If _NeedColumnRecalc Then Return

            _NeedColumnRecalc = True
            If Not OptionsView.ColumnAutoWidth Then
                RecalculateColumnWidths()
            End If
            _NeedColumnRecalc = False
        End Sub
#Region "OnCustomButtonClick"
        ''' <summary>
        ''' Triggers the CustomButtonClick event.
        ''' </summary>
        Public Overridable Sub OnFilterPanelCustomButtonClick(ByVal b As EditorButton, ByVal ea As EventArgs)
            Dim handler As EventHandler = CustomFilterPanelButtonClickEvent
            If handler IsNot Nothing Then
                handler(b, ea)
            End If
        End Sub
#End Region


        Public Sub ShowSortIndexInColumnHeader(e As ColumnHeaderCustomDrawEventArgs)
            If e Is Nothing OrElse e.Column Is Nothing Then Return
            If Not Me.OptionsView.ShowSortIndexInColumnHeader OrElse Me.SortedColumns.Count <= 1 OrElse e.Column.SortIndex = -1 Then Return
            Dim args As GridColumnInfoArgs = e.Info
            e.Painter.DrawObject(args)
            Dim sortArgs As SortedShapeObjectInfoArgs = CType(args.InnerElements.Find(GetType(SortedShapeObjectInfoArgs)).ElementInfo, SortedShapeObjectInfoArgs)
            Dim sortRectangle As Rectangle = sortArgs.Bounds
            Dim sortIndexRectangle As New Rectangle(sortRectangle.X - 8, args.CaptionRect.Y, 10, args.CaptionRect.Height)
            Dim brush As SolidBrush = TryCast(e.Appearance.GetBackBrush(e.Cache), SolidBrush)
            brush.Color = Color.FromArgb(150, brush.Color)
            Dim sortIndexShapeRectangle As New Rectangle(sortIndexRectangle.X - 2, sortIndexRectangle.Y, sortIndexRectangle.Width + sortRectangle.Width, sortIndexRectangle.Height)
            e.Graphics.FillRectangle(brush, sortIndexShapeRectangle)
            e.Appearance.DrawString(e.Cache, e.Column.SortIndex.ToString(), sortIndexRectangle)
            e.Handled = True
        End Sub

        <Browsable(False), XtraSerializableProperty(XtraSerializationVisibility.Collection, True, True, True, 0, XtraSerializationFlags.DefaultValue), XtraSerializablePropertyId(2), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)>
        Public Shadows ReadOnly Property Columns() As GridExColumnCollection
            Get
                Return TryCast(MyBase.Columns, GridExColumnCollection)
            End Get
        End Property
        Public ReadOnly Property FilterPanelButtonsStore As Dictionary(Of EditorButton, EditorButtonObjectInfoArgs)
            Get
                If _FilterPanelButtonsStore Is Nothing Then
                    _FilterPanelButtonsStore = New Dictionary(Of EditorButton, EditorButtonObjectInfoArgs)
                End If
                Return _FilterPanelButtonsStore
            End Get
        End Property
        Public ReadOnly Property FindColumns() As List(Of IDataColumnInfo)

        Public Property FindControl As FindControlEx

        Public Shadows ReadOnly Property OptionsFilter() As GridViewOptionsFilterEx
            Get
                Return TryCast(MyBase.OptionsFilter, GridViewOptionsFilterEx)
            End Get
        End Property

        <XtraSerializableProperty(XtraSerializationVisibility.Content, XtraSerializationFlags.SuppressDefaultValue), XtraSerializablePropertyId(LayoutIdOptionsView)>
        Public Shadows ReadOnly Property OptionsFind() As GridViewOptionsFindEx
            Get
                Return TryCast(MyBase.OptionsFind, GridViewOptionsFindEx)
            End Get
        End Property

        Public ReadOnly Property ScrollInfoEx As ScrollInfo
            Get
                Return Me.ScrollInfo
            End Get
        End Property

#Region "Options"

        <XtraSerializableProperty(XtraSerializationVisibility.Content, XtraSerializationFlags.SuppressDefaultValue), XtraSerializablePropertyId(LayoutIdOptionsView)>
        Public Shadows ReadOnly Property OptionsBehavior As GridOptionsBehaviorEx
            Get
                Return TryCast(MyBase.OptionsBehavior, GridOptionsBehaviorEx)
            End Get
        End Property

        Protected Overrides Function CreateOptionsBehavior() As ColumnViewOptionsBehavior
            Return New GridOptionsBehaviorEx(Me)

        End Function

        <XtraSerializableProperty(XtraSerializationVisibility.Content, XtraSerializationFlags.SuppressDefaultValue), XtraSerializablePropertyId(LayoutIdOptionsView)>
        Public Shadows ReadOnly Property OptionsView As GridOptionsViewEx
            Get
                Return TryCast(MyBase.OptionsView, GridOptionsViewEx)
            End Get
        End Property

        Protected Overrides Function CreateOptionsView() As ColumnViewOptionsView
            Return New GridOptionsViewEx(Me)
        End Function


#End Region


#Region "Extra Panel"

        <Browsable(False)>
        Public ReadOnly Property SplitterControl As SplitterControl
        <Browsable(False)>
        Public ReadOnly Property XtraPanel As XtraScrollableControl

        Private _FilterExApplying As Boolean

        Public Sub RequestXtraPanelControl()
            If _FindControl IsNot Nothing Then
                _FindControl.Dock = DockStyle.Top
                _FindControl.SendToBack()
            End If

            _XtraPanel = New XtraScrollableControl
            _XtraPanel.Dock = DockStyle.Top
            GridControl.Controls.Add(_XtraPanel)
            _XtraPanel.MinimumSize = New Size(0, Me.OptionsView.XtraPanelMinimumHight)
            _XtraPanel.MaximumSize = New Size(0, Me.OptionsView.XtraPanelMaximumHight)
            _XtraPanel.SendToBack()
            _XtraPanel.Visible = Me.OptionsView.ShowXtraPanel

            Me.CreateFilterEditorControl()
            _XtraPanel.Controls.Add(Me.FilterEditorControl)
            Me.FilterEditorControl.Dock = DockStyle.Fill
            AddHandler Me.FilterEditorControl.FilterChanged,
                Sub(s, a)
                    If _FilterExApplying Then Return
                    _FilterExApplying = True
                    If Not Me.FilterEditorControl.HasSkippedCriterias Then
                        Try

                            Me.FilterEditorControl.ApplyFilter()
                        Catch ex As Exception
                            Dim errors = ex

                        End Try
                    End If
                    _FilterExApplying = False
                End Sub
            AddHandler Me.FilterEditorControl.FilterTextChanged,
                Sub(s, a)
                    'If a.IsValid Then
                    If _FilterExApplying Then Return
                    _FilterExApplying = True
                    'If Me.FilterEditorControl.CanBeDisplayedByTreeEx() Then
                    Try
                        Me.FilterEditorControl.ApplyFilter()

                    Catch ex As Exception
                        Dim errors = ex
                    End Try
                    'End If

                    'Me.FilterEditorControl.ApplyFilter()
                    _FilterExApplying = False
                    'End If
                End Sub

            _SplitterControl = New SplitterControl
            _SplitterControl.Dock = DockStyle.Top
            GridControl.Controls.Add(_SplitterControl)
            _SplitterControl.BringToFront()
            _SplitterControl.Visible = Me.OptionsView.ShowXtraPanel

            'customControl_Renamed = New FilterPanel(Me, FilterColumns)
            'customControl_Renamed.Visible = True
            'GridControl.Controls.Add(customControl_Renamed)

            AddHandler Me.SplitterControl.MouseDoubleClick, AddressOf SplitterControl_MouseDoubleClick
            AddHandler Me.SplitterControl.SplitterMoved, AddressOf SplitterControl_SplitterMoved
            'customControl_Renamed.Dock = DockStyle.Top
            'customControl_Renamed.SendToBack()

        End Sub

        Protected Overrides Function CreateDataController() As BaseGridController
            Return MyBase.CreateDataController()
        End Function

        'Private FilterEditorControl As FilterEditorControl
        Public ReadOnly Property FilterEditorControl As FilterEditorControlEx

        Public Function CreateFilterEditorControl() As FilterEditorControlEx
            _FilterEditorControl = New FilterEditorControlEx
            _FilterEditorControl.SourceControl = Me.GridControl
            Return _FilterEditorControl
        End Function

        Private Sub SplitterControl_MouseDoubleClick(sender As Object, e As MouseEventArgs)
            If e.Button = MouseButtons.Right Then Return
            _XtraPanel.Height = Me.OptionsView.XtraPanelMinimumHight
            Me.VisualClientUpdateLayout()
        End Sub

        Private Sub SplitterControl_SplitterMoved(sender As Object, e As SplitterEventArgs)
            Me.VisualClientUpdateLayout()
        End Sub
#End Region

    End Class
End Namespace

