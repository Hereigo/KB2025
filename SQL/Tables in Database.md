# See Tables in DB:

```sql
SELECT TABLE_NAME 
FROM !!_YOUR_DB_NAME_!!.INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE'
AND NAME LIKE '%user%'
```

To find table names in a Microsoft SQL Server database that contain a column with a specific name (including special characters), you can use a dynamic SQL query against the system catalog views. Here’s how you can do it in T-SQL:

### **Method 1: Using `INFORMATION_SCHEMA.COLUMNS`**
This is the most standard and portable way:

```sql
SELECT
    TABLE_NAME
FROM
    INFORMATION_SCHEMA.COLUMNS
WHERE
    COLUMN_NAME = 'YourColumnName';
```

If your column name contains special characters (like spaces, hyphens, etc.), enclose it in square brackets:

```sql
SELECT
    TABLE_NAME
FROM
    INFORMATION_SCHEMA.COLUMNS
WHERE
    COLUMN_NAME = 'Your[Special]Column-Name';
```

---

### **Method 2: Using `sys.columns` and `sys.tables`**
This is more SQL Server-specific and allows for more flexibility:

```sql
SELECT
    t.name AS TableName
FROM
    sys.columns c
INNER JOIN
    sys.tables t ON c.object_id = t.object_id
WHERE
    c.name = 'YourColumnName';
```

For special characters, use square brackets:

```sql
SELECT
    t.name AS TableName
FROM
    sys.columns c
INNER JOIN
    sys.tables t ON c.object_id = t.object_id
WHERE
    c.name = 'Your[Special]Column-Name';
```

---

### **Dynamic SQL for Wildcard Search**

```sql
SELECT
    t.name AS TableName,
    c.name AS ColumnName
FROM
    sys.columns c
INNER JOIN
    sys.tables t ON c.object_id = t.object_id
WHERE
    c.name LIKE '%email%';
```

Or, if the column name is case-sensitive or contains special characters:
```sql
SELECT
    t.name AS TableName
FROM
    sys.columns c
INNER JOIN
    sys.tables t ON c.object_id = t.object_id
WHERE
    c.name = 'user-name';
```

---

### **Notes**
- Replace `'YourColumnName'` or `'Your[Special]Column-Name'` with the actual column name you are searching for.
- If the column name contains special characters, always enclose it in square brackets (`[]`) in SQL Server.
- For case-sensitive searches, use a case-sensitive collation in the `WHERE` clause.