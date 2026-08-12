To use sessions in **Express.js** with `express-session`, you need to configure the middleware and then access the session object in your routes. Here’s a clear step-by-step guide:

---

## 🔧 Setup

1. **Install the package**
   ```bash
   npm install express-session
   ```

2. **Import and configure**
   ```javascript
   import express from 'express';
   import session from 'express-session';

   const app = express();

   app.use(session({
     secret: 'yourSecretKey',   // used to sign the session ID cookie
     resave: false,             // don't save session if unmodified
     saveUninitialized: true,   // save new sessions
     cookie: { secure: false }  // set to true if using HTTPS
   }));
   ```

---

## 📌 Using Session in Routes

```javascript
app.get('/set-session', (req, res) => {
  req.session.username = 'Andrew';
  res.send('Session value set!');
});

app.get('/get-session', (req, res) => {
  if (req.session.username) {
    res.send(`Hello, ${req.session.username}`);
  } else {
    res.send('No session found');
  }
});

app.get('/destroy-session', (req, res) => {
  req.session.destroy(err => {
    if (err) {
      return res.send('Error destroying session');
    }
    res.send('Session destroyed');
  });
});
```

---

## ⚠️ Notes
- **`secret`** should be a long, random string. In production, store it securely (e.g., environment variable).
- **`cookie.secure`** must be `true` if your app runs over HTTPS.
- By default, sessions are stored in memory (not suitable for production). Use a store like **connect-mongo** or **redis** for scalability.

---

👉 Do you want me to show you how to **connect `express-session` with MongoDB** (so sessions persist across server restarts)? That’s the usual next step when working with clusters and databases.