using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AnalysisServices.AdomdClient;
using System.Data;

namespace Lib.ADOMDNET
{
    public class CubesViewer
    {
        /// <summary>
        /// 
        /// </summary>
        private AdomdConnection con = null;


        private string conString = null;


        private bool isOpen = false;
        /// <summary>
        /// 获取或设置AdomdConnection
        /// </summary>
        public AdomdConnection Connection
        {
            get { return con; }
            set { con = value; }
        }
        /// <summary>
        /// 获取AdomdConnection连接状态
        /// </summary>
        public bool IsOpen
        {
            get { return isOpen; }
        }
        /// <summary>
        /// 获取或设置AdomdConnection连接字符串
        /// </summary>
        public string ConnectionString
        {
            get { return conString; }
            set
            {
                conString = value;
                con.ConnectionString = conString;

            }
        }
        /// <summary>
        /// 打开Adomd连接
        /// </summary>
        public void OpenConnection()
        {
            this.con.Open();
            this.isOpen = true;
        }
        /// <summary>
        /// 关闭Adomd连接
        /// </summary>
        public void CloseConnection()
        {
            this.con.Close();

        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public CubesViewer()
        {
            con = new AdomdConnection();
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        public CubesViewer(String connectionString)
        {
            this.conString = connectionString;
            con = new AdomdConnection(conString);
        }
        /// <summary>
        /// 获取所有立方体的信息
        /// </summary>
        /// <param name="cubs">数据库所有立方休信息</param>
        /// <returns>所有立方体的信息</returns>
        public static DataTable Cubes(CubeCollection cubs)
        {

            DataTable table = new DataTable("Cubes");
            table.Columns.Add("Name");
            table.Columns.Add("Type");
            DataRow row;
            for (int i = 0; i < cubs.Count; i++)
            {
                row = table.NewRow();
                row["Name"] = cubs[i].Name;
                row["Type"] = cubs[i].Type.ToString();

                table.Rows.Add(row);
            }
            return table;

        }
        /// <summary>
        /// 获取多维数据集信息
        /// </summary>
        /// <param name="cubes">数据库所有立方休信息</param>
        /// <returns>多维数据集信息</returns>
        public static DataTable CubesCube(CubeCollection cubes)
        {
            DataTable table = new DataTable("Cubers");
            table.Columns.Add(new DataColumn("Name"));

            table.Columns.Add(new DataColumn("Caption"));
            DataRow row;
            foreach (CubeDef cub in cubes)
            {
                if (cub.Type.Equals(CubeType.Cube))
                {


                    row = table.NewRow();
                    row["Name"] = cub.Name;
                    row["Caption"] = cub.Caption;
                    table.Rows.Add(row);

                }

            }

            return table;
        }
        /// <summary>
        /// 获取所有度量值信息
        /// </summary>
        /// <param name="cubes">数据库所有立方休信息</param>
        /// <returns>所有度量值信息</returns>
        public static DataTable CubesMeasures(CubeCollection cubes)
        {
            DataTable table = new DataTable("Measurs");
            table.Columns.Add(new DataColumn("Name"));
            table.Columns.Add(new DataColumn("UniqueName"));
            table.Columns.Add(new DataColumn("Caption"));
            table.Columns.Add(new DataColumn("CubeName"));
            table.Columns.Add(new DataColumn("CuberCaption"));


            DataRow row;

            foreach (CubeDef cub in cubes)
            {
                if (cub.Type.Equals(CubeType.Cube))
                {
                    foreach (Measure measure in cub.Measures)
                    {

                        row = table.NewRow();
                        row["CubeName"] = cub.Name;
                        row["CuberCaption"] = cub.Caption;
                        row["Name"] = measure.Name;
                        row["UniqueName"] = measure.UniqueName;
                        row["Caption"] = measure.Caption;
                        table.Rows.Add(row);
                    }
                }


            }

            return table;



        }
        /// <summary>
        /// 获取所有维度信息
        /// </summary>
        /// <param name="cubes">数据库所有立方休信息</param>
        /// <returns>所有维度信息</returns>
        public static DataTable CubesDimensions(CubeCollection cubes)
        {
            DataTable table = new DataTable("Dimensions");
            table.Columns.Add(new DataColumn("Name"));
            table.Columns.Add(new DataColumn("UniqueName"));
            table.Columns.Add(new DataColumn("Caption"));
            table.Columns.Add(new DataColumn("CubeName"));
            table.Columns.Add(new DataColumn("CuberCaption"));

            DataRow row;
            string name = null;

            foreach (CubeDef cub in cubes)
            {

                if (cub.Type.Equals(CubeType.Dimension))
                {
                    name = cub.Name;
                    name = name.Replace("$", "");
                    Dimension dim = cub.Dimensions[name];
                    row = table.NewRow();
                    row["CubeName"] = cub.Name;
                    row["CuberCaption"] = cub.Caption;
                    row["Name"] = dim.Name;
                    row["UniqueName"] = dim.UniqueName;
                    row["Caption"] = dim.Caption;
                    table.Rows.Add(row);

                }


            }

            return table;
        }
        /// <summary>
        /// 获取所有层次结构信息
        /// </summary>
        /// <param name="cubes">数据库所有立方休信息</param>
        /// <returns>所有层次结构信息</returns>
        public static DataTable CubesHiberarchies(CubeCollection cubes)
        {
            DataTable table = new DataTable("CubeHiberachies");
            table.Columns.Add(new DataColumn("CubeName"));
            table.Columns.Add(new DataColumn("DimName"));

            table.Columns.Add(new DataColumn("UniqueName"));
            table.Columns.Add(new DataColumn("Name"));
            table.Columns.Add(new DataColumn("Caption"));

            DataRow row = null;
            string name = null;
            foreach (CubeDef cub in cubes)
            {
                if (cub.Type.Equals(CubeType.Dimension))
                {
                    name = cub.Name;
                    name = name.Replace("$", "");
                    Dimension dim = cub.Dimensions[name];
                    foreach (Hierarchy hierarchy in dim.Hierarchies)
                    {
                        row = table.NewRow();
                        row["CubeName"] = cub.Name;
                        row["DimName"] = dim.UniqueName;
                        row["UniqueName"] = hierarchy.UniqueName;
                        row["Name"] = hierarchy.Name;
                        row["Caption"] = hierarchy.Caption;
                        table.Rows.Add(row);
                    }

                }
            }
            return table;


        }
        /// <summary>
        /// 获取维度的层次结构信息
        /// </summary>
        /// <param name="dim">维度</param>
        /// <returns>维度的层次结构信息</returns>

        public static DataTable DimHiberarchies(Dimension dim)
        {
            DataTable table = new DataTable("DimHiberachies");

            table.Columns.Add(new DataColumn("DimName"));
            table.Columns.Add(new DataColumn("UniqueName"));
            table.Columns.Add(new DataColumn("Name"));
            table.Columns.Add(new DataColumn("Caption"));

            DataRow row = null;


            foreach (Hierarchy hierarchy in dim.Hierarchies)
            {
                row = table.NewRow();
                row["DimName"] = dim.UniqueName;
                row["UniqueName"] = hierarchy.UniqueName;
                row["Name"] = hierarchy.Name;
                row["Caption"] = hierarchy.Caption;
                table.Rows.Add(row);
            }

            return table;


        }
        /// <summary>
        /// 获取所有级别信息
        /// </summary>
        /// <param name="cubes">数据库所有立方休信息</param>
        /// <returns>所有级别信息</returns>
        public static DataTable CubesLevels(CubeCollection cubes)
        {
            DataTable table = new DataTable("CubeLevels");
            table.Columns.Add(new DataColumn("CubeName"));
            table.Columns.Add(new DataColumn("DimName"));
            table.Columns.Add(new DataColumn("HierarchyName"));

            table.Columns.Add(new DataColumn("UniqueName"));
            table.Columns.Add(new DataColumn("Name"));
            table.Columns.Add(new DataColumn("Caption"));
            table.Columns.Add(new DataColumn("MemberCount"));

            DataRow row = null;
            string name = null;
            foreach (CubeDef cub in cubes)
            {
                if (cub.Type.Equals(CubeType.Dimension))
                {
                    name = cub.Name;
                    name = name.Replace("$", "");
                    Dimension dim = cub.Dimensions[name];


                    foreach (Hierarchy hierarchy in dim.Hierarchies)
                    {
                        foreach (Level level in hierarchy.Levels)
                        {
                            row = table.NewRow();
                            row["CubeName"] = cub.Name;
                            row["DimName"] = dim.UniqueName;
                            row["HierarchyName"] = hierarchy.UniqueName;
                            row["UniqueName"] = level.UniqueName;
                            row["Name"] = level.Name;
                            row["Caption"] = level.Caption;
                            row["MemberCount"] = level.MemberCount.ToString();
                            table.Rows.Add(row);
                        }
                    }

                }
            }
            return table;

        }
        /// <summary>
        /// 获取层次结构级别信息
        /// </summary>
        /// <param name="hiberarchy">层次结构</param>
        /// <returns>层次结构级别信息</returns>
        public static DataTable HierarchyLevels(Hierarchy hiberarchy)
        {
            DataTable table = new DataTable("HiberarchyLevels");
            table.Columns.Add(new DataColumn("HierarchyName"));
            table.Columns.Add(new DataColumn("UniqueName"));
            table.Columns.Add(new DataColumn("Name"));
            table.Columns.Add(new DataColumn("Caption"));
            table.Columns.Add(new DataColumn("MemberCount"));

            DataRow row = null;


            foreach (Level level in hiberarchy.Levels)
            {
                row = table.NewRow();
                row["HierarchyName"] = hiberarchy.UniqueName;
                row["UniqueName"] = level.UniqueName;
                row["Name"] = level.Name;
                row["Caption"] = level.Caption;
                row["MemberCount"] = level.MemberCount.ToString();
                table.Rows.Add(row);
            }

            return table;

        }

        /// <summary>
        /// 获取所有级别成员的信息
        /// </summary>
        /// <param name="cubes">数据库所有立方休信息</param>
        /// <returns>所有级别成员的信息</returns>
        public static DataTable CubesMembers(CubeCollection cubes)
        {
            DataTable table = new DataTable("CubeMembers");
            table.Columns.Add(new DataColumn("DimName"));
            table.Columns.Add(new DataColumn("HierarchieName"));
            table.Columns.Add(new DataColumn("LevelName"));
            table.Columns.Add(new DataColumn("Caption"));
            table.Columns.Add(new DataColumn("UniqueName"));
            table.Columns.Add(new DataColumn("Name"));
            table.Columns.Add(new DataColumn("LevelDepth"));
            table.Columns.Add(new DataColumn("ChildCount"));
            table.Columns.Add(new DataColumn("DrilledDown"));
            DataRow row = null;
            string name = null;
            foreach (CubeDef cub in cubes)
            {

                if (cub.Type.Equals(CubeType.Dimension))
                {
                    name = cub.Name;
                    name = name.Replace("$", "");
                    Dimension dim = cub.Dimensions[name];

                    foreach (Hierarchy hierarchy in dim.Hierarchies)
                    {

                        foreach (Level level in hierarchy.Levels)
                        {
                            foreach (Member member in level.GetMembers())
                            {


                                row = table.NewRow();
                                row["DimName"] = dim.UniqueName;
                                row["HierarchieName"] = hierarchy.UniqueName;
                                row["LevelName"] = member.LevelName;
                                row["UniqueName"] = member.UniqueName;
                                row["Name"] = member.Name;
                                row["LevelDepth"] = member.LevelDepth.ToString();
                                row["ChildCount"] = member.ChildCount.ToString();
                                row["DrilledDown"] = member.DrilledDown;
                                table.Rows.Add(row);

                            }
                        }

                    }
                }
            }

            return table;


        }
        /// <summary>
        /// 获取层次结构级别成员信息
        /// </summary>
        /// <param name="level">层次结构级别</param>
        /// <returns>层次结构级别成员信息</returns>
        public static DataTable LevelMembers(Level level)
        {
            DataTable table = new DataTable("CubeMembers");
            table.Columns.Add(new DataColumn("LevelName"));
            table.Columns.Add(new DataColumn("Caption"));
            table.Columns.Add(new DataColumn("UniqueName"));
            table.Columns.Add(new DataColumn("Name"));

            table.Columns.Add(new DataColumn("LevelDepth"));
            table.Columns.Add(new DataColumn("ChildCount"));

            DataRow row = null;

            foreach (Member member in level.GetMembers())
            {
                row = table.NewRow();
                row["LevelName"] = member.LevelName;
                row["UniqueName"] = member.UniqueName;
                row["Name"] = member.Name;
                row["Caption"] = member.Caption;
                row["LevelDepth"] = member.LevelDepth.ToString();
                row["ChildCount"] = member.ChildCount.ToString();

                table.Rows.Add(row);

            }
            return table;


        }
        /// <summary>
        /// 获取所有命名集信息
        /// </summary>
        /// <param name="cubes">数据库所有立方休信息</param>
        /// <returns>所有命名集信息</returns>
        public static DataTable CubesNameSet(CubeCollection cubes)
        {

            DataTable table = new DataTable("CubeNameSet");
            table.Columns.Add(new DataColumn("Name"));
            table.Columns.Add(new DataColumn("CubeName"));
            table.Columns.Add(new DataColumn("Description"));
            table.Columns.Add(new DataColumn("Expression"));
            DataRow row = null;

            foreach (CubeDef cub in cubes)
            {
                foreach (NamedSet name in cub.NamedSets)
                {
                    row = table.NewRow();
                    row["Name"] = name.Name;
                    row["CubeName"] = cub.Name;
                    row["Description"] = name.Description;
                    row["Expression"] = name.Expression;
                    table.Rows.Add(row);

                }

            }
            return table;


        }
        /// <summary>
        /// 获取所有KPI信息
        /// </summary>
        /// <param name="cubes">数据库所有立方休信息</param>
        /// <returns>所有KPI信息</returns>
        public static DataTable CubesKpiNames(CubeCollection cubes)
        {
            DataTable table = new DataTable("CubeKpis");
            table.Columns.Add("Name");
            table.Columns.Add("Caption");
            table.Columns.Add("StatusGraphic");
            table.Columns.Add("TrendGraphic");

            table.Columns.Add(new DataColumn("CubeName"));
            table.Columns.Add(new DataColumn("CuberCaption"));
            DataRow row = null;
            foreach (CubeDef cub in cubes)
            {
                foreach (Kpi kpi in cub.Kpis)
                {
                    row = table.NewRow();
                    row["Name"] = kpi.Name;
                    row["Caption"] = kpi.Caption;
                    row["StatusGraphic"] = kpi.StatusGraphic;
                    row["TrendGraphic"] = kpi.TrendGraphic;
                    row["CubeName"] = cub.Name;
                    row["CuberCaption"] = cub.Caption;
                    table.Rows.Add(row);

                }
            }
            return table;

        }
        /// <summary>
        /// 获取维度对象
        /// </summary>
        /// <param name="cubes">数据库所有立方休信息</param>
        /// <param name="dimName">维度名称</param>
        /// <returns>维度对象</returns>
        public static Dimension GetDimByName(CubeCollection cubes, string dimName)
        {
            Dimension dim = null;

            string cubeName = "$" + dimName;
            if (cubes[cubeName] != null)
            {
                dim = cubes[cubeName].Dimensions[dimName];

            }
            return dim;
        }
        /// <summary>
        /// 获取层次结构对象
        /// </summary>
        /// <param name="cubes">数据库所有立方休信息</param>
        /// <param name="dimName">维度名称</param>
        /// <param name="hiberarchyName">层次结构名称</param>
        /// <returns>层次结构对象</returns>
        public static Hierarchy GetHierarchyByName(CubeCollection cubes, string dimName, string hiberarchyName)
        {
            Hierarchy hiberarchy = null;
            string cubeName = "$" + dimName;
            if (cubes[cubeName] != null)
            {
                hiberarchy = cubes[cubeName].Dimensions[dimName].Hierarchies[hiberarchyName];

            }
            return hiberarchy;



        }
        /// <summary>
        /// 获取层次结构的级别
        /// </summary>
        /// <param name="cubes">数据库所有立方休信息</param>
        /// <param name="dimName">维度名称</param>
        /// <param name="hiberarchyName">层次结构名称</param>
        /// <param name="levelName">层次强构级别名称</param>
        /// <returns>层次强构级别对象</returns>
        public static Level GetLevelByName(CubeCollection cubes, string dimName, string hiberarchyName, string levelName)
        {
            Level level = null;
            string cubeName = "$" + dimName;
            if (cubes[cubeName] != null)
            {
                level = cubes[cubeName].Dimensions[dimName].Hierarchies[hiberarchyName].Levels[levelName];

            }
            return level;



        }




    }

    public class AdomdHelper : IDisposable
    {
        #region Private variables
        private bool _needClearParameter = false;
        private static string connStr = Lib.Utils.ConfigHelper.GetAppSetting("ADOMDConncectionString");
        #endregion

        #region Properties
        private AdomdConnection _adomdConnection;
        private AdomdConnection Connection
        {
            get
            {
                if (_adomdConnection == null)
                {
                    string adomdConnectionString = connStr;
                    if (string.IsNullOrEmpty(adomdConnectionString))
                        throw new Exception("ADODM.NET' Connectionstring Configuration error!");

                    _adomdConnection = new AdomdConnection(adomdConnectionString);
                }

                return _adomdConnection;
            }
        }
        private AdomdCommand _adomdCommand;
        private AdomdCommand Command
        {
            get
            {
                if (_adomdCommand == null)
                    _adomdCommand = Connection.CreateCommand();

                return _adomdCommand;
            }
        }
        #endregion

        #region Constructor
        public AdomdHelper()
        {
            _adomdConnection = null;
            _adomdCommand = null;
        }

        #endregion

        #region Private methods
        private void ClearParametersInternal()
        {
            if (_needClearParameter)
            {
                Command.Parameters.Clear();
                _needClearParameter = false;
            }
        }
        private void OpenConnection()
        {
            if (Connection.State == ConnectionState.Closed)
                Connection.Open();
        }
        private string ReplaceCubeName(string p_strMDX)
        {
            //if (!string.IsNullOrEmpty(p_strMDX))
            //{
            //    p_strMDX = p_strMDX.Replace("$Cube$", WebDigConfigurations.CubeName);
            //
            return p_strMDX;
        }
        #endregion

        #region Public methods
        public void AddParameter(string p_strParameterName, object p_objParameterValue)
        {
            ClearParametersInternal();
            Command.Parameters.Add(new AdomdParameter(p_strParameterName, p_objParameterValue));
        }

        public void ClearParameters()
        {
            if (_adomdCommand != null)
                _adomdCommand.Parameters.Clear();
        }

        public CellSet ExecuteCellSet(string p_strCommandText)
        {
            p_strCommandText = this.ReplaceCubeName(p_strCommandText);
            CellSet newSet = null;
            if (!string.IsNullOrEmpty(p_strCommandText))
            {
                Command.CommandText = p_strCommandText;
                this.OpenConnection();
                newSet = Command.ExecuteCellSet();
                Connection.Close();
            }
            _needClearParameter = true;
            return newSet;
        }

        public AdomdDataReader ExecuteReader(string p_strCommandText)
        {
            p_strCommandText = this.ReplaceCubeName(p_strCommandText);
            AdomdDataReader reader = null;
            if (!string.IsNullOrEmpty(p_strCommandText))
            {
                Command.CommandText = p_strCommandText;
                this.OpenConnection();
                reader = Command.ExecuteReader();
            }
            _needClearParameter = true;
            return reader;
        }

        #endregion

        #region static methods
        /// <summary>
        /// 将CellSet转化成Table
        /// </summary>
        /// <param name="cellset">CellSet</param>
        /// <returns></returns>
        public static DataTable CellSetToTable(CellSet cs)
        {
            #region
            //DataTable table = new DataTable("cellset");

            //Axis columns = cellset.Axes[0]; //获取列轴
            //Axis rows = cellset.Axes[1];//获取行轴
            //CellCollection valuesCell = cellset.Cells;//获取度量值单元集合
            ////行轴的级别标题为表的第一列
            ////table.Columns.Add(rows.Set.Hierarchies[0].Caption);
            ////行轴的各个成员的标题变成表的列
            //for (int i = 0; i < columns.Set.Tuples.Count; i++)
            //{
            //    table.Columns.Add(new DataColumn(columns.Set.Tuples[i].Members[0].Caption));
            //}
            //int valueIndex = 0;
            //DataRow row = null;
            ////向表中填充数据
            //for (int i = 0; i < rows.Set.Tuples.Count; i++)
            //{
            //    row = table.NewRow();
            //    //表所有行的第一列值为相应行轴的成标题
            //    row[0] = rows.Set.Tuples[i].Members[0].Caption;
            //    for (int k = 1; k <= columns.Set.Tuples.Count; k++)
            //    {//按顺序把度量值单元集合的值填充到表中
            //        row[k] = valuesCell[valueIndex].Value;
            //        valueIndex++;
            //    }
            //    table.Rows.Add(row);
            //}
            //return table;
            #endregion
            DataTable dt = new DataTable();
            dt.TableName = "resulttable";
            DataColumn dc = new DataColumn();
            DataRow dr = null;

            //第一列：必有为维度描述（行头）
            dt.Columns.Add(new DataColumn("Description"));

            //生成数据列对象
            string name;

            foreach (Position p in cs.Axes[0].Positions)
            {
                dc = new DataColumn();
                name = "";
                foreach (Member m in p.Members)
                {
                    name = name + m.Caption + " ";
                }

                dc.ColumnName = name;
                dt.Columns.Add(dc);
            }

            //添加行数据
            int pos = 0;

            foreach (Position py in cs.Axes[1].Positions)
            {
                dr = dt.NewRow();

                //维度描述列数据（行头）
                name = "";

                foreach (Member m in py.Members)
                {
                    name = name + m.Caption + "\r\n";
                }
                dr[0] = name;

                //数据列
                for (int x = 1; x <= cs.Axes[0].Positions.Count; x++)
                {
                    dr[x] = cs[pos++].FormattedValue;
                }
                dt.Rows.Add(dr);
            }
            return dt;

        }

        public static DataTable CellSetToTable_ALLDimention(CellSet cs)
        {
            DataTable dt = new DataTable();
            dt.TableName = "ResultTable";
            DataColumn dc = null;
            DataRow dr = null;

            //生成数据列对象  
            //多个维度转化成列  
            for (int col = 0; col < cs.Axes[1].Set.Hierarchies.Count; col++)
            {
                dc = new DataColumn();
                //下面的代码会报错："The connection is not open.” 获取层次结构的维度名时需要连接Cube才可以！  
                //dt.Columns.Add(new DataColumn(cs.Axes[1].Set.Hierarchies[col].ParentDimension.Name));  
                dt.Columns.Add(new DataColumn("Dimension" + col.ToString()));
            }

            int index = 0;
            foreach (Position p in cs.Axes[0].Positions)
            {
                dc = new DataColumn();
                string name = "";
                foreach (Member m in p.Members)
                {
                    name += m.Caption + "-";
                }
                if (name.Length > 0)
                {
                    name = name.Substring(0, name.Length - 1);
                }
                //这里防止维度成员或度量值重名而需要容错处理  
                try
                {
                    dc.ColumnName = name;
                    dt.Columns.Add(dc);
                }
                catch (System.Exception ex)
                {
                    dc.ColumnName = name + index.ToString();
                    dt.Columns.Add(dc);
                }
                index++;
            }

            //添加行数据  
            int pos = 0;
            foreach (Position py in cs.Axes[1].Positions)
            {
                dr = dt.NewRow();

                //维度描述列数据  
                int cols = 0;
                foreach (Member m in py.Members)
                {
                    dr[cols] = m.Caption;
                    cols++;
                }

                //数据列  
                for (int x = 1; x <= cs.Axes[0].Positions.Count; x++)
                {
                    dr[x + cols - 1] = cs[pos++].FormattedValue;
                }
                dt.Rows.Add(dr);
            }
            return dt;

        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (_adomdCommand != null)
            {
                _adomdCommand.Dispose();
                _adomdCommand = null;
            }

            if (_adomdConnection != null)
            {
                if (_adomdConnection.State != ConnectionState.Closed)
                    _adomdConnection.Close();

                _adomdConnection.Dispose();
                _adomdConnection = null;
            }
        }

        #endregion
    }
}
