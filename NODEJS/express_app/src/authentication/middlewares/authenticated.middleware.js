import jsonwebtoken from 'jsonwebtoken';
import { NotAuthenticatedError } from '../../errors/models/not-authenticated-errror.model.js';
import { WEB_TOKEN_SECRET_KEY } from '../../../config.js';

export const authenticated = (req, res, next) => {
    try {

        console.log('authenticated - ', req.cookies);

        // const token = req.session.token;
        const token = req.cookies.token;
    
        // jsonwebtoken.decode(a,b); - Doesn't verify signature or expiration!
        jsonwebtoken.verify(token, WEB_TOKEN_SECRET_KEY);

        return next();
    } catch (error) {
        next(new NotAuthenticatedError());
    }
};
