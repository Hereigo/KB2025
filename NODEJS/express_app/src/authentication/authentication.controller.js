import { getRoleByUserLogin } from '../users/users.service.js';
import * as authenticationService from './authentication.service.js';

export const signIn = async (req, res, next) => {
    try {
        const { login, password } = req.body;

        const token = await authenticationService.authenticateUser(login, password);

        res.cookie('token', token, { expires: new Date(Date.now() + 20 * 60 * 60 * 1000), httpOnly: true });
        // To restrict sending Cookies only via HTTPS: ---------------------------------> httpOnly: true, secure: true});


        // ?????????
        // TODO:
        // req.session.token = token;
        // req.session.role = await getRoleByUserLogin(login);

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
