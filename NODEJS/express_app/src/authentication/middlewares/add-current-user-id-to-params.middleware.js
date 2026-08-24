import jsonwebtoken from 'jsonwebtoken';
import { WEB_TOKEN_SECRET_KEY } from '../../../config.js';

export const addCurrentUserIdToParams = (req, res, next) => {
    try {

        // const token = req.cookies.token;
        const token = req.session.token;
        
        // Souldn't use jsonwebtoken.decode(a,b); - Doesn't verify signature or expiration!
        const decoded = jsonwebtoken.verify(token, WEB_TOKEN_SECRET_KEY);

        req.params.id = decoded.id;

        return next();
    } catch (error) {
        next(error);
    }
};