require("dotenv").config();

const { MongoClient, ServerApiVersion } = require('mongodb');

const uri = process.env.MONGODB_URI;
const dbName = process.env.MONGODB_DBNAME;

// Create a MongoClient with a MongoClientOptions object to set the Stable API version
const client = new MongoClient(uri, {
    serverApi: {
        version: ServerApiVersion.v1,
        strict: true,
        deprecationErrors: true,
    }
});

async function main() {
    try {
        await client.connect();
        // Send a ping to confirm a successful connection
        await client.db("admin").command({ ping: 1 });
        console.log("Pinged your deployment. You successfully connected to MongoDB!");

        // Example: insert a document
        const db = client.db(dbName);
        const collection = db.collection("users");

        const result = await collection.insertOne({ name: "Alice", age: 25 });
        console.log("Inserted document:", result.insertedId);

        const users = await collection.find().toArray();
        console.log("Users:", users);

    } catch (err) {
        console.error("Error connecting to MongoDB:", err);
    } finally {
        // Ensures that the client will close when you finish/error
        await client.close();
    }
}

main();
