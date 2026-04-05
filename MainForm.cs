using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TVChannelsHacked
{
    public sealed class MainForm : Form
    {
        private readonly TextBox _searchBox = new TextBox();
        private readonly ComboBox _countryFilter = new ComboBox();
        private readonly ComboBox _typeFilter = new ComboBox();
        private readonly DataGridView _grid = new DataGridView();
        private readonly CheckedListBox _videoSelector = new CheckedListBox();
        private readonly ListBox _comingNext = new ListBox();
        private readonly TextBox _summary = new TextBox();
        private readonly Label _status = new Label();
        private readonly Button _showSelectedButton = new Button();

        private readonly List<ChannelIncident> _all = IncidentRepository.BuildSeedData();

        public MainForm()
        {
            Text = "TV Channels Live Stream & Satellite Incident Browser";
            MinimumSize = new Size(1150, 700);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

            BuildLayout();
            BuildFilters();
            WireEvents();
            RefreshGrid();
        }

        private void BuildLayout()
        {
            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 3,
                Padding = new Padding(10)
            };

            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62f));
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38f));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 60f));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 32f));
            Controls.Add(root);

            var filters = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                WrapContents = false,
                FlowDirection = FlowDirection.LeftToRight
            };

            filters.Controls.Add(new Label { Text = "Search:", AutoSize = true, Padding = new Padding(0, 8, 0, 0) });
            _searchBox.Width = 230;
            filters.Controls.Add(_searchBox);

            filters.Controls.Add(new Label { Text = "Country:", AutoSize = true, Padding = new Padding(10, 8, 0, 0) });
            _countryFilter.Width = 170;
            _countryFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            filters.Controls.Add(_countryFilter);

            filters.Controls.Add(new Label { Text = "Type:", AutoSize = true, Padding = new Padding(10, 8, 0, 0) });
            _typeFilter.Width = 170;
            _typeFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            filters.Controls.Add(_typeFilter);

            root.Controls.Add(filters, 0, 0);
            root.SetColumnSpan(filters, 2);

            _grid.Dock = DockStyle.Fill;
            _grid.AutoGenerateColumns = false;
            _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _grid.MultiSelect = false;
            _grid.AllowUserToAddRows = false;
            _grid.AllowUserToDeleteRows = false;
            _grid.ReadOnly = true;
            _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Channel", DataPropertyName = nameof(ChannelIncident.ChannelName), Width = 220 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Country", DataPropertyName = nameof(ChannelIncident.Country), Width = 150 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Region", DataPropertyName = nameof(ChannelIncident.Region), Width = 90 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Incident Type", DataPropertyName = nameof(ChannelIncident.IncidentType), Width = 210 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "UTC", DataPropertyName = nameof(ChannelIncident.IncidentTimeUtc), Width = 140 });
            root.Controls.Add(_grid, 0, 1);

            var rightPane = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 5,
                ColumnCount = 1,
                Padding = new Padding(8)
            };
            rightPane.RowStyles.Add(new RowStyle(SizeType.Absolute, 26f));
            rightPane.RowStyles.Add(new RowStyle(SizeType.Percent, 45f));
            rightPane.RowStyles.Add(new RowStyle(SizeType.Absolute, 36f));
            rightPane.RowStyles.Add(new RowStyle(SizeType.Percent, 35f));
            rightPane.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));

            rightPane.Controls.Add(new Label { Text = "Sources Videos (multi-select)", Dock = DockStyle.Fill, TextAlign = ContentAlignment.BottomLeft }, 0, 0);

            _videoSelector.Dock = DockStyle.Fill;
            _videoSelector.CheckOnClick = true;
            rightPane.Controls.Add(_videoSelector, 0, 1);

            _showSelectedButton.Text = "Show Selected Sources";
            _showSelectedButton.Dock = DockStyle.Fill;
            rightPane.Controls.Add(_showSelectedButton, 0, 2);

            _comingNext.Dock = DockStyle.Fill;
            rightPane.Controls.Add(_comingNext, 0, 3);

            _summary.Multiline = true;
            _summary.ReadOnly = true;
            _summary.Dock = DockStyle.Fill;
            _summary.ScrollBars = ScrollBars.Vertical;
            rightPane.Controls.Add(_summary, 0, 4);

            root.Controls.Add(rightPane, 1, 1);

            _status.Dock = DockStyle.Fill;
            _status.TextAlign = ContentAlignment.MiddleLeft;
            _status.ForeColor = Color.DarkSlateBlue;
            root.Controls.Add(_status, 0, 2);
            root.SetColumnSpan(_status, 2);
        }

        private void BuildFilters()
        {
            _countryFilter.Items.Clear();
            _typeFilter.Items.Clear();

            _countryFilter.Items.Add("All countries");
            _typeFilter.Items.Add("All types");

            foreach (var country in _all.Select(x => x.Country).Distinct().OrderBy(x => x))
            {
                _countryFilter.Items.Add(country);
            }

            foreach (var type in _all.Select(x => x.ContentType).Distinct().OrderBy(x => x))
            {
                _typeFilter.Items.Add(type);
            }

            _countryFilter.SelectedIndex = 0;
            _typeFilter.SelectedIndex = 0;
        }

        private void WireEvents()
        {
            _searchBox.TextChanged += (_, __) => RefreshGrid();
            _countryFilter.SelectedIndexChanged += (_, __) => RefreshGrid();
            _typeFilter.SelectedIndexChanged += (_, __) => RefreshGrid();
            _grid.SelectionChanged += (_, __) => BindSelected();
            _showSelectedButton.Click += (_, __) => ShowSelectedSources();
        }

        private void RefreshGrid()
        {
            IEnumerable<ChannelIncident> filtered = _all;
            var search = _searchBox.Text.Trim();

            if (!string.IsNullOrWhiteSpace(search))
            {
                filtered = filtered.Where(i =>
                    i.ChannelName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    i.Summary.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    i.IncidentType.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            if (_countryFilter.SelectedIndex > 0 && _countryFilter.SelectedItem is string country)
            {
                filtered = filtered.Where(i => i.Country.Equals(country, StringComparison.OrdinalIgnoreCase));
            }

            if (_typeFilter.SelectedIndex > 0 && _typeFilter.SelectedItem is string type)
            {
                filtered = filtered.Where(i => i.ContentType.Equals(type, StringComparison.OrdinalIgnoreCase));
            }

            var list = filtered
                .OrderByDescending(x => x.IncidentTimeUtc)
                .ToList();

            _grid.DataSource = list;
            _status.Text = $"Loaded {list.Count} incidents | Windows 8.1-compatible target: .NET Framework 4.8";

            if (list.Count > 0)
            {
                _grid.Rows[0].Selected = true;
                BindSelected();
            }
            else
            {
                _videoSelector.Items.Clear();
                _comingNext.Items.Clear();
                _summary.Text = "No incident matched your filters.";
            }
        }

        private void BindSelected()
        {
            if (_grid.CurrentRow?.DataBoundItem is not ChannelIncident incident)
            {
                return;
            }

            _videoSelector.Items.Clear();
            foreach (var video in incident.SourceVideos)
            {
                _videoSelector.Items.Add(video, false);
            }

            _comingNext.Items.Clear();
            _comingNext.Items.Add($"Coming next for {incident.ChannelName}:");
            foreach (var next in incident.ComingNext)
            {
                _comingNext.Items.Add($"• {next}");
            }

            _summary.Text =
                $"Channel: {incident.ChannelName}{Environment.NewLine}" +
                $"Country: {incident.Country} / Region: {incident.Region}{Environment.NewLine}" +
                $"Type: {incident.ContentType}{Environment.NewLine}" +
                $"Incident: {incident.IncidentType}{Environment.NewLine}" +
                $"UTC Time: {incident.IncidentTimeUtc:u}{Environment.NewLine}{Environment.NewLine}" +
                incident.Summary;
        }

        private void ShowSelectedSources()
        {
            if (_videoSelector.CheckedItems.Count == 0)
            {
                MessageBox.Show(this, "Please choose one or more source videos.", "No videos selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selected = string.Join(Environment.NewLine, _videoSelector.CheckedItems.Cast<string>());
            MessageBox.Show(this, $"Selected source videos:{Environment.NewLine}{selected}", "Selection",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
