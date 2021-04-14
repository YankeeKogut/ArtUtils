namespace ArtUtils.Net.Core
{
    public static class Constants
    {
        public const string ErrWrongDataType =
            "The given value of type String from the data source cannot be converted to type int of the specified target column";

        public const string ErrorMessageFixColumnsOrder =
            "Underlying data type error. Can be caused by mismatch in column order between table and entity object";

        public const string ErrorNullRecordInDataSet =
            "Input list has one or more NULL elements.";

        public const string ErrorTableAttributesMissing =
            "Table attributes missing. Please make sure that all objects have TableName and KeyField attributes and ParentKeyField attribute if nested and needed";

        public const string ErrorTableNameAttributeMissing =
            "TableName attributes missing. ";

        public const string DefaultSchemaName = "dbo";

    }
}
