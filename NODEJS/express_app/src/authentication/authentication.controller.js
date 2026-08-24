import { getRoleByUserLogin } from '../users/users.service.js';
import * as authenticationService from './authentication.service.js';

export const signIn = async (req, res, next) => {
    try {
        const { login, password } = req.body;

        const token = await authenticationService.authenticateUser(login, password);

        // let tokenExpires = new Date(Date.now() + 24 * 60 * 60 * 1000);
        let tokenExpires = new Date(Date.now() + 60 * 1000);

        // res.cookie('token', token, { expires: tokenExpires, httpOnly: true });
        // // To restrict sending Cookies only via HTTPS: ---> httpOnly: true, secure: true});

        req.session.token = token;
        req.session.role = await getRoleByUserLogin(login);
        // req.session.expires = ????


        console.log("SIGN IN tokenExpires - ", tokenExpires);

        
        return res.json({});

    } catch (error) {
        return next(error);
    }
};

export const signUp = async (req, res, next) => {
    try {
        const { login, password } = req.body;
        const newUser = await authenticationService.registerNewUser(login, password);
        return res.json(newUser);
    } catch (error) {
        return next(error);
    }
};

export const signOut = async (req, res, next) => {
    // const { login, password } = req.body;

    // const token = await authenticationService.authenticateUser(login, password);

    // Clear the cookie by setting it to null and expiring immediately
    // res.clearCookie('token'); ??????????????????
    req.session.token = null;

    //res.redirect('/login'); // or wherever you want to send the user
    return res.json({ message: 'logged out.' });
};
