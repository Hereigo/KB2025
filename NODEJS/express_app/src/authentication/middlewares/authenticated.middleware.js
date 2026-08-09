import jsonwebtoken from 'jsonwebtoken';
import { NotAuthenticatedError } from '../../errors/models/not-authenticated-errror.model.js';
import { WEB_TOKEN_SECRET_KEY } from '../../../config.js';

export const authenticated = (req, res, next) => {
    try {

        console.log('A - ', req.body);
        // console.log('A - ', res);
        // console.log('A - ', next);

        const { token } = req.body;
    
        jsonwebtoken.verify(token, WEB_TOKEN_SECRET_KEY);

        return next();
    } catch (error) {
        next(new NotAuthenticatedError());
    }
};
