import jsonwebtoken from 'jsonwebtoken';

export const addCurrentUserIdToParams = (req, res, next) => {
    try {
        const { token } = req.body;
    
        const decoded01 = jsonwebtoken.verify(token);
        console.log('d01 - ', decoded01); // ?????
        const decoded = jsonwebtoken.decode(token);
        console.log('d - ', decoded); // ?????

        req.params.id = decoded.id;

        return next();
    } catch (error) {
        next(error);
    }
};