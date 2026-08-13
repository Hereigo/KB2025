import jsonwebtoken from 'jsonwebtoken';
import { WEB_TOKEN_SECRET_KEY } from '../../../config.js';

export const addCurrentUserIdToParams = (req, res, next) => {
    try {

        // const { token } = req.body;

    console.log('addCurrentUserIdToParams - ', req.cookies)

        // const token = req.session.token;
        const token = req.cookies.token;
    
        // Souldn't use jsonwebtoken.decode(a,b); - Doesn't verify signature or expiration!
        // const decoded = jsonwebtoken.verify(token, WEB_TOKEN_SECRET_KEY);

        const decoded = jsonwebtoken.decode(token); // ???

        req.params.id = decoded.id;

        return next();
    } catch (error) {
        next(error);
    }
};