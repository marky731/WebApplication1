import json
import psycopg2

# Connect to PostgreSQL
conn = psycopg2.connect(
    dbname="mydb",
    user="postgres",
    password="1234",
    host="localhost",
    port="5432"
)
cursor = conn.cursor()

# Load JSON file
with open("Users.json", "r") as file:
    users = json.load(file)

# Insert data into PostgreSQL
for user in users:
    cursor.execute(
        """
        INSERT INTO public.users ("Id", "Firstname", "Surname", "Gender") 
        VALUES (%s, %s, %s, %s)
        """,
        (user["UserId"]+1002, user["FirstName"], user["LastName"], user["Gender"])
    )

# Commit and close connection
conn.commit()
cursor.close()
conn.close()
