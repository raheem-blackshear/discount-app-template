using DAL.Models.Questions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DAL.Repositories.Interfaces
{
    class IQuestionRepository : IRepository<Question>
    {
        public void Add(Question entity)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<Question> entities)
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Question> Find(Expression<Func<Question, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Question Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Question> GetAll()
        {
            throw new NotImplementedException();
        }

        public Question GetSingleOrDefault(Expression<Func<Question, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void Remove(Question entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<Question> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(Question entity)
        {
            throw new NotImplementedException();
        }

        public void UpdateRange(IEnumerable<Question> entities)
        {
            throw new NotImplementedException();
        }
        

    }
}
